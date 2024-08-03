using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    public Vector2 inputs; //Inputs need to be determined by other scripts
    public float moveModifier = 1;
    public float timeUntilSlide = 0.42f;
    public bool immobilized = false;
    public bool knockbackCooldown;
    public bool slideAvailable = false;

    private Rigidbody2D playerRB;
    public HealthSystem objectHealth;

    private float durRestricted;
    // Start is called before the first frame update
    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        objectHealth = GetComponent<HealthSystem>();
        objectHealth.movementScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        //do this first so that the play input is normalized, but not things like movespeed. Normalizing accounts for diagonal speed boosts, but also sets all velocity you could gain, to 1;
        //Immobilization just means "cant control your velocity" it doesnt mean that you cant move
        if(!immobilized)
        {
            playerRB.velocity = new Vector2((moveSpeed * inputs.x) * moveModifier, (moveSpeed * inputs.y) * moveModifier);
        }
        else if(slideAvailable) //its ok to run so many else ifs every frame. Sliding is a rare occurence;
        {
            if((inputs.x + inputs.y) != 0)
            {
                UnrestrictMovement();
            }
        }
    }

    public void RestrictMovement(float timeRestricted, bool canSlide)
    {
        immobilized = true;
        if(canSlide)
        {
            Invoke("AllowSlide", timeUntilSlide);
        }
        else
        {
            CancelInvoke("AllowSlide");
            canSlide = false;
        }
        durRestricted = timeRestricted;
        CancelInvoke("UnrestrictMovement");
        Invoke("UnrestrictMovement", timeRestricted * 0.4f);
        knockbackCooldown = true;
        Invoke("KnockbackCooldownReset", 0.01f);
    }

    public void UnrestrictMovement()
    {
        objectHealth.StoredKnockbackApply();   //Stored knockback only comes into play with parries. SO: normal collisions that would cause restricted movement dont care about stored knockback;
        //playerRB.velocity = new Vector2(playerRB.velocity.x - playerRB, playerRB.velocity.y)
        playerRB.drag = 22;
        playerRB.AddForce(new Vector2(-1 * (playerRB.velocity.x * 0.1f), -1 * (playerRB.velocity.y * 0.1f)), ForceMode2D.Force);
        Invoke("DeImmobilize", durRestricted);
        slideAvailable = false;
    }
    
    private void DeImmobilize()
    {
        playerRB.drag = 1;
        immobilized = false;        
    }

    public void KnockbackCooldownReset()
    {
        knockbackCooldown = false;
    }

    public void AllowSlide()
    {
        slideAvailable = true;
    }
}
