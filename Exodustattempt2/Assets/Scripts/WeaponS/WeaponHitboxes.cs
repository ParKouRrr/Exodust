using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxes : MonoBehaviour
{
    // The final evolution of OnCollisionEnter
    public bool isEnemy; //is determined during runtime
    public bool dealsDamage;
    public bool knocksBack;
    public bool blocking;
    public float damage;
    public float knockbackDuration = 1f;
    public float knockbackAmount = 5f;
    public int damageLayer = 0;
    public Rigidbody2D playerRB;
    public WeaponScript weaponScript;
    public Transform playerTransform;
    public HealthSystem contactedHPSystem;
    public EntityFX playerFX;
    public GameObject player;
    public Collider2D playerCollider;

    // Parrying
    public bool deflectProjectilesImmediatley;     //Does nothing atm
    public float parryKnockbackDuration;
    public float parryKnockbackAmount;
    public float parryDamage;
    public float parryDelay; //The amount of seconds after successful parry, that more attacks are allowed to be parried.
    public float timeSlowAllowence;

    public GameObject[] parriedObjects;

    public HealthSystem playerHealth;
    // Start is called before the first frame update
    void Awake()
    {
        weaponScript = transform.parent.GetComponent<WeaponScript>();
        if(weaponScript.ownerObject.tag != "Player")
        {
            isEnemy = true;
        }
        if(!isEnemy)
        {
            player = GameObject.FindWithTag("Player");                  //Yeah im not refactoring all that. "player" now means "wielder"
            timeSlowAllowence = 100; //time slow is chosen in editor if entity is not a player
        }
        else
        {
            player = weaponScript.ownerObject;
        }
        playerRB = player.GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<HealthSystem>();
        playerTransform = player.transform;
        playerFX = GameObject.FindWithTag("Player").GetComponent<EntityFX>();
        playerHealth.parryDelay = parryDelay;
        playerHealth.weaponBladeScript = this;
        playerCollider = player.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if(collision.gameObject.GetComponent<HealthSystem>())
        {
            contactedHPSystem = collision.gameObject.GetComponent<HealthSystem>();
            contactedHPSystem.storedKnockbackAmount = new Vector2(contactedHPSystem.storedKnockbackAmount.x + (((transform.position.x - collision.gameObject.transform.position.x) * -1
                * parryKnockbackAmount) / 2), contactedHPSystem.storedKnockbackAmount.y + ((transform.position.y - collision.gameObject.transform.position.y) * -1 * parryKnockbackAmount) / 2);
            contactedHPSystem.storedKnockbackDuration = (parryKnockbackDuration / 2);
            if(dealsDamage)
            {
                if(contactedHPSystem.damageLayer != damageLayer)
                {
                    contactedHPSystem.TakeDamage(damage, damageLayer);
                    Debug.Log("Dealt: " +  damage + " to: " + collision.gameObject);
                }
            }
        }        
        //Debug.Log(collision.gameObject.name);
        if(knocksBack)
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>())
            {
                if(collision.gameObject.GetComponent<Movement>())
                {
                    collision.gameObject.GetComponent<Movement>().RestrictMovement(knockbackDuration, false);
                }
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((transform.position.x - collision.gameObject.transform.position.x) * -1
                * knockbackAmount, (transform.position.y - collision.gameObject.transform.position.y) * -1 * knockbackAmount);
            }
        }
        if(blocking)
        {
            if(collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                if(collision.gameObject.tag == "Projectile")
                {
                    if(collision.gameObject.GetComponent<Movement>() != null)
                    {
                        collision.gameObject.GetComponent<Movement>().RestrictMovement(10, false);
                        collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
                        Array.Resize(ref parriedObjects, parriedObjects.Length + 1);
                        parriedObjects[(parriedObjects.Length - 1)] = collision.gameObject;
                        ParryProjectile(collision.gameObject);
                        //playerFX.slowTime(0.16f, 0.5); It just feels worse to slow time when parrying weak projectiles
                    }                    
                }
            }
            //else      //this is carried out by hp
            //{        

               // weaponScript.thisAnimController.speed = 1;
                //LaunchParriedObjects();
                //playerRB.gameObject.GetComponent<Movement>().RestrictMovement(playerHealth.storedKnockbackDuration / 2);
                //playerRB.velocity = new Vector2((playerTransform.position.x - playerTransform.position.x) * -1
                //* (playerHealth.storedKnockbackAmount / 2), (playerTransform.position.y - playerTransform.position.y) * -1 * (playerHealth.storedKnockbackAmount / 2));
                //playerFX.slowTime(0.2f, 0);
            //}
        }
        contactedHPSystem = null;
    }
    public void ParryProjectile(GameObject projectile)
    {
        projectile.GetComponent<OnTriggerEnterEffects>().damageLayer = 1;
        projectile.GetComponent<Movement>().RestrictMovement(parryKnockbackDuration, false);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2((transform.position.x - projectile.transform.position.x) * -1
            * parryKnockbackAmount, (transform.position.y - projectile.transform.position.y) * -1 * parryKnockbackAmount);
    }
    public void ParryAttack()
    {
        //playerFX.slowTime(0.46f, 0.0f, timeSlowAllowence); Good, now that youve found time slow in the parry script: YOU NEED TO ADD TIME SLOW IN THE PARRY ANIMATION IDIOT
        weaponScript.thisAnimController.SetFloat("AtkSpeed", weaponScript.baseAtkSpeed * weaponScript.AtkSpeedModifier);
        weaponScript.thisAnimController.Play("BasicParry");
        playerHealth.storedDamage = 0;
        lastApplicator = null;
        LaunchParriedObjects();
    }
    public void LaunchParriedObjects()
    {
        //playerFX.slowTime(0.3f, 0.5f, timeSlowAllowence);
        CancelInvoke("ParryAttack");
        foreach (GameObject i in parriedObjects)
        {
            if(i != null)
            {
                i.GetComponent<Movement>().RestrictMovement(parryKnockbackDuration, false);
                i.GetComponent<Rigidbody2D>().velocity = new Vector2((transform.position.x - i.transform.position.x) * -1
                * parryKnockbackAmount, (transform.position.y - i.transform.position.y) * -1 * parryKnockbackAmount);                     
            }
        }
        Array.Resize(ref parriedObjects, 1);
        parriedObjects[0] = null;
    }

    private GameObject lastApplicator;
    public void PassVarsToHealth(Vector2 kAmount, float kDuration, float exDamage, int damageLayer, GameObject applicator)
    {
        if(lastApplicator != applicator)
        {
            playerHealth.storedKnockbackAmount = new Vector2(playerHealth.storedKnockbackAmount.x + kAmount.x,
            playerHealth.storedKnockbackAmount.y + kAmount.y);
            playerHealth.storedKnockbackDuration += kDuration;
            playerHealth.TakeDamage(exDamage, damageLayer);            
        }

        lastApplicator = applicator;
    }
}
