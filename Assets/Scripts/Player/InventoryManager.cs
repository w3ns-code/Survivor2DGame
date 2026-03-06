using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
   public List<WeaponController> weaponSlots = new List<WeaponController>(6);
   public int[] weaponLevels = new int[6];
   public List<Image> weaponUISlots = new List<Image>(6);
   public List<PassiveItem> passiveItemsSlots = new List<PassiveItem>(6);
   public int[] passiveLevels = new int[6];
   public List<Image> passiveItemUISlots = new List<Image>(6);

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite= weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemsSlots[slotIndex] = passiveItem;
        passiveLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite= passiveItem.passiveItemData.Icon;
    }

    public void levelUpWeapon(int slotIndex)
    {
        if(weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No Next Level For  " + weapon.name);
                return;
            }
            GameObject upgradeWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradeWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradeWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradeWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
        
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if(passiveItemsSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemsSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("No Next Level For  " + passiveItem.name);
                return;
            }
            GameObject upgradePassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradePassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradePassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveLevels[slotIndex] = upgradePassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;
        }
    }

}
