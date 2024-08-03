using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{   //!!---------------------------------!!
    //This is the BASE weapon script
    //!!---------------------------------!!
    //For performance reasons, each weapon has its own script, so you will need to change what "thisAnimController" is playing
    //!!---------------------------------!!
    public float damage;
    public float baseAtkSpeed;
    public float AtkSpeedModifier;
    public GameObject ownerObject;
    public Animator thisAnimController;
    public WeaponHitboxes bladeStats;
    [SerializeField] public string primaryInputID;
    [SerializeField] public string secondaryInputID;
    public string primaryFunction;
    public string secondaryFunction;
    public string idleFunction;
    [SerializeField] private string primaryReleaseFunction;
    [SerializeField] private string secondaryReleaseFunction;

    public bool isParrying; //super inneficient way to do this but o well
    //public bool attacking; removed cuz I think it's useless
    // Start is called before the first frame update
    void Awake()
    {
        ownerObject = transform.parent.parent.gameObject;
        bladeStats = transform.GetChild(0).GetComponent<WeaponHitboxes>();
        thisAnimController = GetComponent<Animator>();
        primaryReleaseFunction = primaryFunction + "Release";
        secondaryReleaseFunction = secondaryFunction + "Release";
    }

    public void SetBladeStatsToWeaponStats()
    {
        bladeStats.damage = damage; //not to be confused with damage (damage #1 is blade damage and damage #2 is weapon damage)
    }

    public void Primary()
    {   
        if(primaryFunction != null)
        {
            Invoke(primaryFunction, 0.0f);      //function will be called a frame late  
        }
        SetBladeStatsToWeaponStats();
    }

    public void Secondary()
    {
        if(secondaryFunction != null)
        {
            Invoke(secondaryFunction, 0.0f);      //function will be called a frame late  
        }
        SetBladeStatsToWeaponStats();
    }

    public void PrimaryRelease()
    {
        if(primaryReleaseFunction != null)
        {
            Invoke(primaryReleaseFunction, 0.0f);
        }
    }

    public void SecondaryRelease()
    {
        if(secondaryReleaseFunction != null)
        {
            Invoke(secondaryReleaseFunction, 0.0f);
        }
    }

    public void BasicBlock()
    {
        if(Input.GetButtonDown(secondaryInputID))
        {
        thisAnimController.SetFloat("AtkSpeed", 0);
        thisAnimController.Play("BasicBlock");
        }     
    }

    public void BasicBlockRelease()
    {
        thisAnimController.SetFloat("AtkSpeed", 10);       
    }

    public void BasicSlash()
    {
        thisAnimController.SetFloat("AtkSpeed", baseAtkSpeed * AtkSpeedModifier);
        thisAnimController.Play("BasicSwing");
    }

    public void BasicSlashRelease()
    {
    }

    public void Idle()
    {
        if(idleFunction != null)
        {
            Invoke(idleFunction, 0.0f);            
        }
        thisAnimController.Play("Idle");
    }

    public void MakePlayerInvincible()
    {
        CancelInvoke("PleaseIgnoreHowBadMyCodeIs");
        bladeStats.playerHealth.invincible = true;
    }

    public void StopPlayerInvincible(float secsToWait)
    {
        if(secsToWait != 0)
        {
            Invoke("PleaseIgnoreHowBadMyCodeIs", secsToWait);
        }
        else
        {
            bladeStats.playerHealth.invincible = false;
        }
    }
    
    public void PleaseIgnoreHowBadMyCodeIs()
    {
        bladeStats.playerHealth.invincible = false;        
    }

    public void PauseTime(float pauseDuration)
    {
        bladeStats.playerFX.slowTime(pauseDuration, 0, bladeStats.timeSlowAllowence);
    }

    public void ApplyStoredKnockback(float multiplier)  //multiplier does nothing atm, remove if this doesnt change
    {
        Debug.Log((bladeStats.playerHealth.storedKnockbackAmount.x + bladeStats.playerHealth.storedKnockbackAmount.y) + " |||| " + "   dur:  " + bladeStats.playerHealth.storedKnockbackDuration);
        bladeStats.playerHealth.StoredKnockbackApply();
    }

    //Parrying
    public void EnterParryState() //The state the player is put in when they have the option to perform a parry
    {

    }
    
    public void ExitParryState()
    {

    }
}

