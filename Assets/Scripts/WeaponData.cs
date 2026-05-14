using UnityEngine;

public enum JobClass { Archer, Warrior, Mage, Assassin, Gunner }
public enum DamageType { Physical, Magical, Poison, Explosive, Piercing }
public enum StatusEffect { None, Poison, Burn, Stun, Slow }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "RPG/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    [TextArea] public string description;
    public JobClass jobClass;

    [Header("Combat Stats")]
    public float attackPower;
    public float range;
    [Tooltip("Attacks per second")] public float attackSpeed;

    [Header("Special Conditions")]
    public bool isMelee;
    public bool usesAmmo;
    [Min(0)] public int ammoConsumptionPerShot;
    [Min(0)] public int maxAmmo;
    public float reloadTime;

    [Header("Damage Type")]
    public DamageType damageType;

    [Header("Area of Effect")]
    [Tooltip("0 means no AoE")] public float aoeRadius;

    [Header("Projectile")]
    public float projectileSpeed;
    public float projectileLifetime;

    [Header("Critical Hit")]
    [Range(0f, 1f)] public float criticalChance;
    [Min(1f)] public float criticalMultiplier = 1.5f;

    [Header("Status Effect")]
    public StatusEffect statusEffect;
    public float statusEffectDuration;
    [Tooltip("Poison/Burn: damage per second, Slow: movement speed reduction (0~1)")] public float statusEffectValue;

    [Header("Item Properties")]
    [Tooltip("Does this item disappear after use?")] public bool isConsumable;
    [Tooltip("Can this item be sold in a shop?")] public bool isSellable;
}
