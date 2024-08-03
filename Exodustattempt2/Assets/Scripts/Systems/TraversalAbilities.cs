using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversalAbilities : MonoBehaviour
{   
    public Movement playerMovement;
    public Rigidbody2D playerRb;
    //dash
    public float dashSpeed;
    public float dashTime;
    public Vector2 storedInputs;
    public bool dashing = false;
    public bool dashOnCooldown = false;
    //grapple

    //jump (good luck)

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Movement>();
    }
    void Update()
    {
        //dash
        if(Input.GetKeyDown("left shift"))
        {
            if(!dashOnCooldown)
            {
                playerMovement.immobilized = true;
                dashing = true;
                storedInputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
                Debug.Log("dash");
                Invoke("StopDashing", dashTime);
            }
        }
        if(dashing)
        {
            playerRb.velocity = new Vector2(storedInputs.x * dashSpeed, storedInputs.y * dashSpeed);
        }
        //grapple

        //jump
    }

    public void StopDashing()
    {
        playerMovement.immobilized = false;
        dashing = false;
    }
}
