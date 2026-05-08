using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TurretDemo
{
    /// <summary>
    /// Turret가 인식할 수 있는 범용 Enemy 표식 컴포넌트입니다.
    /// 활성 Enemy 목록을 static registry로 관리합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EnemyTarget : MonoBehaviour
    {
        private static readonly List<EnemyTarget> ActiveTargetsInternal = new List<EnemyTarget>(64);

        [SerializeField]
        [Tooltip("향후 진영 분리 확장을 위한 팀 식별자입니다.")]
        private int teamId;

        [SerializeField]
        [Tooltip("최대 체력입니다.")]
        private float maxHealth = 10f;

        [SerializeField]
        [Tooltip("비워두면 transform.position을 조준점으로 사용합니다.")]
        private Transform aimPoint;

        [SerializeField]
        private float _blinkSpeed = 5f;

        [Header("Death Sequence")]
        [SerializeField]
        [Tooltip("사망 연출에 사용할 Renderer.")]
        private Renderer bodyRenderer;

        [SerializeField]
        [Tooltip("넉백 이동 거리(월드 단위).")]
        private float knockbackDistance = 1.5f;

        private float currentHealth;
        private bool isDying;
        private Vector3 originalScale;

        public static IReadOnlyList<EnemyTarget> ActiveTargets => ActiveTargetsInternal;

        public int TeamId => teamId;

        public float CurrentHealth => currentHealth;

        public Transform CachedTransform => transform;

        public Vector3 AimWorldPosition => aimPoint != null ? aimPoint.position : transform.position;

        private void Awake()
        {
            originalScale = transform.localScale;

            if (bodyRenderer == null)
            {
                bodyRenderer = GetComponentInChildren<Renderer>();
            }
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
            isDying = false;
            transform.localScale = originalScale;

            if (!ActiveTargetsInternal.Contains(this))
            {
                ActiveTargetsInternal.Add(this);
            }
        }

        private void OnDisable()
        {
            ActiveTargetsInternal.Remove(this);
        }

        public bool ApplyDamage(float damageAmount, Vector3 hitDirection = default)
        {
            if (damageAmount <= 0f || isDying)
            {
                return false;
            }

            currentHealth -= damageAmount;
            if (currentHealth <= 0f)
            {
                isDying = true;
                ActiveTargetsInternal.Remove(this);
                StartCoroutine(DieSequence(hitDirection));
            }

            return true;
        }

        private IEnumerator DieSequence(Vector3 hitDirection)
        {
            var mover = GetComponent<EnemyLinearMover>();
            if (mover != null)
            {
                mover.enabled = false;
            }

            // 1. 피격 플래시: 빨간 → 흰색 (0.05초)
            if (bodyRenderer != null)
            {
                Material mat = bodyRenderer.material;
                float elapsed = 0f;
                const float flashDuration = 0.05f;
                while (elapsed < flashDuration)
                {
                    elapsed += Time.deltaTime;
                    mat.color = Color.Lerp(Color.red, Color.white, elapsed / flashDuration);
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }

            // 2. 넉백 모션 (0.1초)
            if (hitDirection != Vector3.zero)
            {
                float elapsed = 0f;
                const float knockbackDuration = 0.1f;
                Vector3 startPos = transform.position;
                Vector3 endPos = startPos + hitDirection * knockbackDistance;
                while (elapsed < knockbackDuration)
                {
                    elapsed += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPos, endPos, elapsed / knockbackDuration);
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            // 3. 스케일 수축 (0.15초)
            {
                float elapsed = 0f;
                const float shrinkDuration = 0.15f;
                Vector3 startScale = transform.localScale;
                while (elapsed < shrinkDuration)
                {
                    elapsed += Time.deltaTime;
                    transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / shrinkDuration);
                    yield return null;
                }
            }

            Destroy(gameObject);
        }


        public void ActivateTarget()
        {
            gameObject.SetActive(true);
            StartCoroutine(ActivateTargetRoutine());
        }
        
        IEnumerator ActivateTargetRoutine()
        {
            float elapsed = 0f;
            const float shrinkDuration = 2f;

            Vector3 finalizeScale = transform.localScale;
            transform.localScale = Vector3.zero;

            while (elapsed < shrinkDuration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.zero, finalizeScale, elapsed / shrinkDuration);
                yield return null;
            }
            
        }
    }

    
}
