using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContactEffects : MonoBehaviour
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<HealthSystem>())
        {
            contactedHPSystem = collision.gameObject.GetComponent<HealthSystem>();
            contactedHPSystem.storedKnockbackAmount = new Vector2(contactedHPSystem.storedKnockbackAmount.x + (((transform.position.x - collision.gameObject.transform.position.x) * -1
                * knockbackAmount) / 2), contactedHPSystem.storedKnockbackAmount.y + ((transform.position.y - collision.gameObject.transform.position.y) * -1 * knockbackAmount) / 2);
            contactedHPSystem.storedKnockbackDuration += (knockbackDuration / 2);
            if(dealsDamage)
            {
                if(contactedHPSystem.damageLayer != damageLayer)
                {
                    contactedHPSystem.TakeDamage(damage, damageLayer);
                }
            }
        }
        else if(collision.gameObject.GetComponent<WeaponHitboxes>().blocking)
        {
            if(dealsDamage)
            {
                collision.gameObject.GetComponent<WeaponHitboxes>().PassVarsToHealth(new Vector2(contactedHPSystem.storedKnockbackAmount.x + (((transform.position.x - collision.gameObject.transform.position.x)
                * -1 * knockbackAmount) / 2), contactedHPSystem.storedKnockbackAmount.y + ((transform.position.y - collision.gameObject.transform.position.y)
                * -1 * knockbackAmount) / 2), knockbackDuration, damage, damageLayer, this.gameObject);      
            }
            else
            {
                collision.gameObject.GetComponent<WeaponHitboxes>().PassVarsToHealth(new Vector2(contactedHPSystem.storedKnockbackAmount.x + (((transform.position.x - collision.gameObject.transform.position.x)
                * -1 * knockbackAmount) / 2), contactedHPSystem.storedKnockbackAmount.y + ((transform.position.y - collision.gameObject.transform.position.y)
                * -1 * knockbackAmount) / 2), knockbackDuration, 0, damageLayer, this.gameObject);           
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
