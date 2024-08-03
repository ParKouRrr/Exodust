using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldsCrappiestAi : MonoBehaviour
{
    public float turningSpeed; //in degrees per second
    public Transform playerTransform;
    public Vector2 playerPos;
    public Vector2 currentInputs;
    public Movement movement;
    public Vector2 previousInputs;
    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
        movement.inputs = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y).normalized;
    }
}
