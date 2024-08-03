using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalExplosion : MonoBehaviour
{
    public bool implodeFirst; //does nothing for now
    public bool needsActivation;

    public float thisRadius;
    public float targetRadius;
    public float thisDamage; //the direct damage of the explosion is covered by oncontactfx
    public float thisBadassery;
    public float growthTime;
    public float lifetime;

    [SerializeField] Transform thisTransform;

    [SerializeField] CircleCollider2D collider;
    // Start is called before the first frame update
    void Awake()
    {   
        thisTransform = GetComponent<Transform>();
        collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
        if(!needsActivation)
        {
            Explosion(thisRadius, thisDamage, thisBadassery);
        }
    }

    // Update is called once per frame
    void Update()
    {
        thisRadius = thisRadius + ((targetRadius) * Time.deltaTime / growthTime);
        thisRadius = Mathf.Clamp(thisRadius, 1.0f, targetRadius);
        transform.localScale = new Vector3(thisRadius, thisRadius, thisRadius);          
    }

    public void Explosion(float radius, float damage, float badassery)
    {
        collider.enabled = true;
        targetRadius = radius;
        thisRadius = 0;
        Invoke("DestroyThis", lifetime);

    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
