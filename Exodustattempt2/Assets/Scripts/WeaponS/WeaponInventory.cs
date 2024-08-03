using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public GameObject pivot;   //MUST BE DECLARED IN EDITOR (cause im lazy :3)
    public GameObject[] weaponInventory;
    public WeaponIDStorage idStorage;
    public GameObject selectedWeapon;
    public int maxSlots;
    public int[] startingWeaponIDS;

    public bool isEnemy;
    // Start is called before the first frame update
    void Awake()
    {
        maxSlots = weaponInventory.Length - 1;
        idStorage = GameObject.FindWithTag("Pivot").GetComponent<WeaponIDStorage>();
        for(int i = 0; i <= maxSlots; i++) //if I is still smaller than the max slots, increase its value and then run: 
        {
            //I++ is run at the END of this block (i starts at 0)
            if(!isEnemy)    //if it isnt an enemy run:
            {
                if(i <= startingWeaponIDS.Length - 1) //if the I value is still lower than the amount of starting weapons
                {     
                    weaponInventory[i] = idStorage.weaponIDs[startingWeaponIDS[i]];  //replace wep 0 with fists at some point;         //fills I slot in this weapon inventory with the prefab which has a matching ID with the I starting weapon    
                }
                else  //if its not lower than the amount of starting weapons, instead
                {
                    weaponInventory[i] = idStorage.weaponIDs[startingWeaponIDS[startingWeaponIDS.Length - 1]];    //it helps to make both arrays the same length     fills all unused slots with the ending weapon
                }
            }
            else
            {
                if(i <= startingWeaponIDS.Length - 1)
                {
                    weaponInventory[i] = idStorage.enemyWeaponIDS[startingWeaponIDS[i]];                   
                }
                else
                {
                    weaponInventory[i] = idStorage.enemyWeaponIDS[startingWeaponIDS[startingWeaponIDS.Length - 1]];    //REMEMBER: .Length RETURNS THE "normal people" number you have to subtract one or it wont work with larger array lengths
                }
            }
        }
        TakeOutWeapon(0);
    }

    public void TakeOutWeapon(int inventorySlot)
    {
        Destroy(selectedWeapon);
        if(inventorySlot <= idStorage.weaponIDs.Length)
        {
            if(!isEnemy)
            {
                //Switches Player Animator
            }
            selectedWeapon = Instantiate(weaponInventory[inventorySlot], pivot.transform, worldPositionStays:false);
        }
    }
}
