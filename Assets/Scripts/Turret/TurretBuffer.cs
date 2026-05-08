using System.Collections;
using TMPro;
using TurretDemo;
using UnityEngine;
using UnityEngine.UI;

public class TurretBuffer : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _currentEnergy = 100f;
    [SerializeField] private float _consumeSpeed = 5f;
    [SerializeField] private float _rechargeSpeed = 5f;
    [SerializeField] private bool _canUseAbility = true;

    [Header("Buff")]
    [SerializeField] private float _multiple = 1.5f;

    [Header("UI")]
    [SerializeField] private Slider _energySlider;
    [SerializeField] private TextMeshProUGUI _energyValue;
    [SerializeField] private TextMeshProUGUI _tooltips;

    private BaseTurretController _turret;

    private void Awake()
    {
        _turret = GetComponent<BaseTurretController>();

        _currentEnergy = _maxEnergy;

        _energySlider.maxValue = _maxEnergy;
        _energySlider.value = _currentEnergy;

        UpdateEnergyUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryBuffTurretAbility();
        }
    }

    private void TryBuffTurretAbility()
    {
        if (!_canUseAbility)
        {
            return;
        }

        if (_currentEnergy < _maxEnergy)
        {
            return;
        }

        StartCoroutine(BuffRoutine());
    }

    private IEnumerator BuffRoutine()
    {
        _canUseAbility = false;

        float originYaw = _turret.YawSpeedDegreesPerSecond;
        float originPitch = _turret.PitchSpeedDegreesPerSecond;
        float originProjectileSpeed = _turret.ProjectileSpeed;

        _turret.YawSpeedDegreesPerSecond *= _multiple;
        _turret.PitchSpeedDegreesPerSecond *= _multiple;
        _turret.ProjectileSpeed *= _multiple;
        UpdateTooltips("Buffered..");
        while (_currentEnergy > 0f)
        {
            _currentEnergy -= _consumeSpeed * Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0f, _maxEnergy);

            UpdateEnergyUI();

            yield return null;
        }
        UpdateTooltips("Recharging..");
        _turret.YawSpeedDegreesPerSecond = originYaw;
        _turret.PitchSpeedDegreesPerSecond = originPitch;
        _turret.ProjectileSpeed = originProjectileSpeed;

        while (_currentEnergy < _maxEnergy)
        {
            _currentEnergy += _rechargeSpeed * Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0f, _maxEnergy);

            UpdateEnergyUI();

            yield return null;
        }

        _canUseAbility = true;
        UpdateTooltips("Press Q (Buff)");
    }

    private void UpdateEnergyUI()
    {
        _energySlider.value = _currentEnergy;
        _energyValue.text = $"{(int)_currentEnergy}";
    }

    private void UpdateTooltips(string msg)
    {
        _tooltips.text = msg;
    }

}