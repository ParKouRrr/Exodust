using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A script for getting misc inputs to avoid putting IF statements where I dont have too.
public class InputTracking : MonoBehaviour
{   
    //This is the base input tracking script. Its current functionality is meant for the player
    public WeaponInventory playerInventory;
    public string leftClickID;
    public string rightClickID;
    public Movement playerMovement;
    
    // Start is called before the first frame update
    void Awake()
    {
        leftClickID = "LeftClick";
        rightClickID = "RightClick";
        playerMovement = GetComponent<Movement>();
    }
    // Update is called once per frame
    void Update()
    {
        playerMovement.inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        //Attacks
        if(Input.GetButtonDown(leftClickID))
        {
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().Primary();
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().primaryInputID = leftClickID;
        }
        if(Input.GetButtonUp(leftClickID))
        {
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().PrimaryRelease();
        }
        if(Input.GetButton(rightClickID))
        {
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().Secondary();
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().secondaryInputID = rightClickID;
        }
        if(Input.GetButtonUp(rightClickID))
        {
            playerInventory.selectedWeapon.GetComponent<WeaponScript>().SecondaryRelease();
            Debug.Log("RightClickUp");
        }
        //Inventory NEEDS to get redone
        if(Input.GetKeyDown("1"))
        {
            playerInventory.TakeOutWeapon(0);
        }
        if(Input.GetKeyDown("2"))
        {
            playerInventory.TakeOutWeapon(1);
        }
        if(Input.GetKeyDown("3"))
        {
            playerInventory.TakeOutWeapon(2);
        }
    }
}
