using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterEffects : MonoBehaviour
{
    public bool dealsDamage;
    public bool knocksBack;
    public bool suicideOnContact;
    public bool suicideOnTerrain;
    public string tagToSuicideOn;
    public float damage;
    public float knockbackDuration = 1f;
    public float knockbackAmount = 5f;
    public int damageLayer = 0;

    public HealthSystem contactedHPSystem;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<HealthSystem>())
        {
            contactedHPSystem = collision.gameObject.GetComponent<HealthSystem>();
            if(!contactedHPSystem.invincible)
            {
                if(dealsDamage)
                {
                    if(contactedHPSystem.damageLayer != damageLayer)
                    {
                        contactedHPSystem.TakeDamage(damage, damageLayer);
                    }
                }
                if(knocksBack)
                {
                    if(collision.gameObject.GetComponent<Rigidbody2D>())
                    {
                        if(collision.gameObject.GetComponent<Movement>())
                        {
                            collision.gameObject.GetComponent<Movement>().RestrictMovement(knockbackDuration, false);
                        }
                        collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(((transform.position.x - collision.gameObject.transform.position.x) * -1
                        * knockbackAmount) / 2, ((transform.position.y - collision.gameObject.transform.position.y) * -1 * knockbackAmount) / 2);
                    }                   
                }
            }
        }
        else if(collision.gameObject.GetComponent<WeaponHitboxes>())
        {
            if(collision.gameObject.GetComponent<WeaponHitboxes>().blocking)
            {
                if(dealsDamage)
                {
                    //only works against players unfortunatley
                    collision.gameObject.GetComponent<WeaponHitboxes>().PassVarsToHealth(new Vector2((((transform.position.x - collision.gameObject.transform.position.x)
                    * -1 * knockbackAmount) / 2),((transform.position.y - collision.gameObject.transform.position.y)
                    * -1 * knockbackAmount) / 2), knockbackDuration, damage, damageLayer, this.gameObject);      
                }
                else
                {
                    collision.gameObject.GetComponent<WeaponHitboxes>().PassVarsToHealth(new Vector2((((transform.position.x - collision.gameObject.transform.position.x)
                    * -1 * knockbackAmount) / 2),((transform.position.y - collision.gameObject.transform.position.y)
                    * -1 * knockbackAmount) / 2), knockbackDuration, 0, damageLayer, this.gameObject);        
                }
            }
        }        
        //God help me

        //Debug.Log(collision.gameObject.name);
        // else if(knocksBack)
        // {
        //     if(collision.gameObject.GetComponent<Rigidbody2D>())
        //     {
        //         Rigidbody2D contactRB = collision.gameObject.GetComponent<Rigidbody2D>();
        //         if(collision.gameObject.GetComponent<Movement>())
        //         {
        //             collision.gameObject.GetComponent<Movement>().RestrictMovement(knockbackDuration, false);
        //         }
        //         contactRB.velocity = new Vector2((transform.position.x - collision.gameObject.transform.position.x) * -1
        //         * knockbackAmount * Mathf.Clamp(contactRB.velocity.x * 0.5f, 1, 2), (transform.position.y - collision.gameObject.transform.position.y) * -1
        //          * knockbackAmount * Mathf.Clamp(contactRB.velocity.y * 0.5f, 1, 2));
        //         // old one, might have to revert to
        //         //contactRB.velocity = new Vector2((transform.position.x - collision.gameObject.transform.position.x) * -1
        //         //* knockbackAmount * Mathf.Clamp(contactRB.velocity.x * 0.5f, 1, 2), (transform.position.y - collision.gameObject.transform.position.y) * -1 * knockbackAmount * (contactRB.velocity.y * 0.5f));
        //     }
        // }
        contactedHPSystem = null;
        if(suicideOnContact)
        {
            Suicide(collision.gameObject, tagToSuicideOn, suicideOnTerrain);
        }

    }
    void Suicide(GameObject collisionObject, string tagToLookFor, bool ignoreTag)
    {
        if(collisionObject.tag == tagToLookFor || ignoreTag)
        {
            if(GetComponent<HealthSystem>())
            {
                GetComponent<HealthSystem>().Die();
            }
            else
            {
                Destroy(this.gameObject); 
            }
        } 
    }
}