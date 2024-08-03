using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public int damageLayer = 0; //basically like the team the object is assigned to
    // 0 = Enemy 1 = Player 2 = Non-Friendly-Fire-Terrain;
    public bool showHPBar = false;
    public bool explodeOnDeath = false;
   
    public float barBaseYOffset;
    public float deathFXDelay = 1f;
    public float deathFXRadius = 3.8f;
    public float deathFXDamage = 40f;
    public float deathFXBadassery = 1f;
    public float timeSlowAllowence = 100;
    //Runtime
    public bool invincible; // Invincible is ONLY for parrying. For invincibility that doesnt trigger a parry when attacked, make a different bool (or name this one better)
    public float maxHP = 100;                                                             //READ THIS  ^^^^
        public float baseHP = 100; //basehp != maxhp
    public float defenseMultiplier;
    public float storedDamage; //Damage taken when invincible
    public Vector2 storedKnockbackAmount;
    public float storedKnockbackDuration; //Knockback taken in general. I need to turn the knockback taken by entities into a float,
    //cuz there's a stupid number of different collision effect scripts and I cant just go look for the knockback of the collider.

    public float parryDelay;

    [SerializeField] bool deathState = false; //You want to count still currently in action entities as dead, so that 
    //ex: the player doesnt trigger another death explosion during the death explosion animation

    [SerializeField] private GameObject HPBar;
    [SerializeField] private Transform barOffset;

    public WeaponHitboxes weaponBladeScript;
    public GameObject explosionPrefab;
    public GameObject summonedExplosion;
    public Movement movementScript; //needs to be delcared by movement script itself

    public void Awake()
    {
        baseHP = maxHP;
    }
    public void Start()
    {
        HPBar = GameObject.FindWithTag("HealthBar");
    }
    public void TakeDamage(float damage, int hitboxDamageLayer)
    {
        if(hitboxDamageLayer != damageLayer)
        {
            if(!invincible) //invincible is impossible to get without a weapon so you dont need checks for a script
            {
                baseHP -= damage;
                if(baseHP <= 0)
                {
                    if(!deathState)
                    {
                        Die();
                        if(damageLayer == 1)
                        {
                            //weaponBladeScript.playerFX.slowTime(2, 0.2f, timeSlowAllowence);
                        }
                        else if((baseHP -= damage) == (maxHP * 1.5f) + 30)
                        {
                            //weaponBladeScript.playerFX.slowTime(Mathf.Clamp(maxHP / ((damage - maxHP) + 30), 0.1f, 1.1f), 0f, timeSlowAllowence);
                        }
                    }
                }
                if(showHPBar)
                {
                    HPBar.GetComponent<HPBarScript>().ChangeSelectedObjects(this.gameObject.transform, barBaseYOffset);
                    UpdateHPBar();
                }                    
            }
            else
            {
                storedDamage += damage;
                weaponBladeScript.CancelInvoke("ParryAttack");
                weaponBladeScript.Invoke("ParryAttack", parryDelay);
            }
        }
    }
    public void Die()
    {
        deathState = true;
        thisSprite = GetComponent<SpriteRenderer>();
        //TODO add example code later on, with this but using shaders instead of the sprite renderer
        InvokeRepeating("increaseColor", 0.04f, 0.04f);
        Invoke("DeathEffects", deathFXDelay);
    }
    //not to be confused with death particles
    public void DeathEffects()
    {
        if(explodeOnDeath)
        {
            DeathExplosion(deathFXRadius, deathFXDamage, deathFXBadassery);
        }
        else
        {
            DestroyThis();
        }
    }

    //-----------Everything in here should be deleted once colors are replaced with shaders------------
    public SpriteRenderer thisSprite;
    public void increaseColor()
    {
        thisSprite.color = new Color(thisSprite.color.r + 0.01f, thisSprite.color.b + 0.01f, thisSprite.color.g + 0.01f);
    }
    //-----------Everything in here should be deleted once colors are replaced with shaders------------

    public void DeathExplosion(float radius, float damage, float badassery) //badassery = screen shake, fx, and destruction multiplier
    {   

        Debug.Log("BOOOOMMM");
        summonedExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        summonedExplosion.GetComponent<UniversalExplosion>().Explosion(radius, damage, badassery);
        DestroyThis();
    }

    public void DestroyThis()
    {
        if(gameObject.tag != "Player")
        {
            if(HPBar.GetComponent<HPBarScript>().selectedTransform == transform)
            {
                HPBar.GetComponent<HPBarScript>().ChangeSelectedObjects(HPBar.GetComponent<Transform>(), barBaseYOffset);
            }        
            Destroy(gameObject);            
        }

    }

    public void UpdateHPBar()
    {
        HPBar.GetComponent<HPBarScript>().UpdateValues(baseHP / maxHP);
    }

    public void StoredDamageApply()
    {
        Debug.Log("AppliedStoredDamage: " + storedDamage);
        storedDamage = 0;
    }

    private Vector3 normalizedMultiplier;
    public void StoredKnockbackApply()
    {
        if(Mathf.Abs(storedKnockbackAmount.x + storedKnockbackAmount.y) > 0)
        {
            normalizedMultiplier = storedKnockbackAmount.normalized;
            movementScript.RestrictMovement(storedKnockbackDuration, true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Abs(normalizedMultiplier.x) * storedKnockbackAmount.x, Mathf.Abs(normalizedMultiplier.y) * storedKnockbackAmount.y);
            storedKnockbackAmount = new Vector3(0, 0, 0);
            storedKnockbackDuration = 0;
            StoredDamageApply();
        }
    }
}
