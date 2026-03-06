using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObjects characterData;

    //Stats Actuels
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]   
    public float currentProjectileSpeed;
    [HideInInspector]   
    public float currentMagnet;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject fpt, spt;

    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(fpt);
        SpawnPassiveItem(spt);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

  void Update()
  {
    if(invencibilityTimer > 0){
        invencibilityTimer -= Time.deltaTime;
    } 
    //si le timer e l'invenciility est de 0
    else if (isInvencible){
        isInvencible = false;        
    }
    Recover();
  }

  //Experince et level
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public float invencibilityDuration;
    float invencibilityTimer;
    bool isInvencible;

    public List<LevelRange> levelRanges;
 

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        levelUpChecker();
    }

    void levelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach(LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvencible)
        {
            currentHealth -= dmg;
            
            invencibilityTimer = invencibilityDuration;
            isInvencible = true;

            if(currentHealth <= 0)
            {
                kill();    
            }
        }
        
    }

    public void kill()
    {
        Debug.Log("YOU ARE DEAD");
    }

    public void RestoreHealth(float health)
    {
        if(currentHealth < characterData.MaxHealth)
        {
           currentHealth += health;

           if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            } 
        }
        
    }

    void Recover()
    {
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if(weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }
        //Spawn l'arme principale
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //Fait que l'arme soit enfant du player
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if(passiveItemIndex >= inventory.passiveItemsSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }
     
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform); 
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }


}
