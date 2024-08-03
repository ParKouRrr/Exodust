using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroHitboxScript : MonoBehaviour
{
    public float hitboxSize;
    public int teamToLookFor; //Declared in editor
    public float aggroOffset = 0.3f; //Declared in editor
    public string tagToLookFor; //Declared in editor, tag takes a little fewer resources to run than id checking
    public string collisionEventFunction; //Declared in editor, "default" is: EnterAggressionState();
    public Transform assignedTransform; //Declared in editor
    public AIBase ai;

    public bool basicAggro;
    // Start is called before the first frame update
    void Awake()
    {
        if(ai == null)
        {
            ai = transform.parent.GetComponent<AIBase>();            
        }
        if(assignedTransform == null)
        {
            assignedTransform = transform.parent.GetComponent<Transform>();
        }
        FigureOutHitboxSize();
        transform.SetParent(null, true);
    }

    void FixedUpdate()
    {
        if(assignedTransform != null)
        {
            transform.position = assignedTransform.position;            
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)  //Projectiles/summons cannot currently alert the hitbox;
    {
        if(collision.gameObject.tag == tagToLookFor)
        {
            if(basicAggro)
            {
                ai.SetAggressionState((int)(Mathf.Abs((transform.position.x - collision.gameObject.transform.position.x) + (transform.position.y - collision.gameObject.transform.position.y)) / (hitboxSize / 2) + aggroOffset));  
                ai.SetAggroedTransform(collision.gameObject.GetComponent<Transform>());              
            }
            else
            {
                ai.Invoke(collisionEventFunction, 0.0f);
            }
        }
    }
    
    private Collider2D thisCollider;
    public void FigureOutHitboxSize()
    {
        thisCollider = GetComponent<Collider2D>();
        hitboxSize = (thisCollider.bounds.size.x + thisCollider.bounds.size.y);
        Debug.Log(hitboxSize);
    }
}
