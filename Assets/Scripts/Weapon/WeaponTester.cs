using UnityEngine;

public class WeaponTester : MonoBehaviour
{
    [SerializeField] private WeaponData[] _weapons;

    void Start()
    {
        PrintAllWeapons();
    }

    private void PrintAllWeapons()
    {
        if (_weapons == null || _weapons.Length == 0)
        {
            Debug.LogWarning("무기 데이터 없음..");
            return;
        }

        for (int i = 0; i < _weapons.Length; i++)
        {
            WeaponData weaponData = _weapons[i];

            if (weaponData == null)
            {
                Debug.LogWarning($"{i}번 무기 슬롯 비어있음..");
                continue;
            }

            Debug.Log(GetLogWeaponData(i, weaponData));
        }
    }

    private string GetLogWeaponData(int num, WeaponData weaponData)
    {
        string log =
        $"[Weapon - {num + 1}]\n\n" +
        $"Name: {weaponData.weaponName} ({weaponData.jobClass})\n" +
        $"Description: {weaponData.description}\n\n" +

        $"--- Combat ---\n" +
        $"Attack Power: {weaponData.attackPower}\n" +
        $"Range: {weaponData.range}\n" +
        $"Attack Speed: {weaponData.attackSpeed} (per second)\n\n" +

        $"--- Type ---\n" +
        $"Damage Type: {weaponData.damageType}\n" +
        $"Is Melee: {weaponData.isMelee}\n\n" +

        $"--- Ammo ---\n" +
        $"Uses Ammo: {weaponData.usesAmmo}\n" +
        $"Ammo/Shot: {weaponData.ammoConsumptionPerShot}\n" +
        $"Max Ammo: {weaponData.maxAmmo}\n" +
        $"Reload Time: {weaponData.reloadTime}\n\n" +

        $"--- AoE / Projectile ---\n" +
        $"AoE Radius: {weaponData.aoeRadius}\n" +
        $"Projectile Speed: {weaponData.projectileSpeed}\n" +
        $"Projectile Lifetime: {weaponData.projectileLifetime}\n\n" +

        $"--- Critical ---\n" +
        $"Critical Chance: {weaponData.criticalChance}\n" +
        $"Critical Multiplier: {weaponData.criticalMultiplier}\n\n" +

        $"--- Status Effect ---\n" +
        $"Effect: {weaponData.statusEffect}\n" +
        $"Duration: {weaponData.statusEffectDuration}\n" +
        $"Value: {weaponData.statusEffectValue}\n\n" +

        $"--- Item ---\n" +
        $"Consumable: {weaponData.isConsumable}\n" +
        $"Sellable: {weaponData.isSellable}\n\n";

        return log;
    }
}