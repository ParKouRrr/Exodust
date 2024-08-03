using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Camera thisCam;
    public float maxLookDistance = 16;
    public float whatSmoothedVelocityCountsBy = 0.1f;
    public float mouseMovementMultiplier; //not sens
    public float playerVelocityMultiplier;
    public float smoothingMultiplier;
    public float fovVelocityMultiplier;

    public float degenRate = 1f;
    //raw values too high to directly multiply anything with. 
    [SerializeField] private float velocityCalcs;
    [SerializeField] private float smoothedVelocityCalcs;
    [SerializeField] public float baseFOV = 10f;
    //the fov to be used later this frame
    [SerializeField] float targetFOV;
    [SerializeField] Transform playerCoords;
 
    void Start()
    {
        smoothedVelocityCalcs = 1;
        thisCam.orthographicSize = baseFOV;
    }

    void LateUpdate()
    {
        //velocity multiplier smoothing, so you dont get TOO MUCH whiplash

        //               a base one multiplier plus the total velocity of the player times a velocity coefficient. Since different framerates would run the same adjustment code, deltatime has to be multiplied at some point. Multiplying it all the way back here however would cause the smoothed value to jump up during lag spikes
        velocityCalcs = (1 + (Mathf.Abs(playerRb.velocity.x) + Mathf.Abs(playerRb.velocity.y)) * playerVelocityMultiplier);
        //                       the previous value of this, minus a framerate-accounted difference between svc and vc, multiplied by yet ANOTHER multiplier, rounded to an integer, and then re-converted to a number that counts by the number defined by a var
        smoothedVelocityCalcs = (Mathf.Round((smoothedVelocityCalcs - ((smoothedVelocityCalcs - velocityCalcs) * Time.deltaTime)  * smoothingMultiplier) / whatSmoothedVelocityCountsBy) * whatSmoothedVelocityCountsBy);
        //           a multiplier based on player speed * a static variable, plus the base fov. the minus one is there because smoothed calcs will always be + 1 in order to work as a multiplier. we dont want a multiplier here tho
        targetFOV = ((smoothedVelocityCalcs - 1) * fovVelocityMultiplier) + baseFOV;
        thisCam.orthographicSize = targetFOV;


        //Absolute stupidity, enter at your own risk.  Basically, I throw every single movement camera trick at the wall, and then divide them all a ton until it just BARELY wont give you epilepsy
        transform.position = new Vector3 (Mathf.Clamp(playerCoords.position.x + ((Input.mousePosition.x - (Screen.width / 2)) / 200) * mouseMovementMultiplier * smoothedVelocityCalcs, playerCoords.position.x - maxLookDistance, playerCoords.position.x + maxLookDistance),
        Mathf.Clamp(playerCoords.position.y + ((Input.mousePosition.y - (Screen.height / 2)) / 200) * mouseMovementMultiplier * smoothedVelocityCalcs, playerCoords.position.y - maxLookDistance, playerCoords.position.y + maxLookDistance), -10);

        //archived movement speed scaling code

        // targetFOV = baseFOV + (Mathf.Abs(playerRb.velocity.x + playerRb.velocity.y) * 0.05f);
        // //adds to camera fov based on player velocity                              /this 0.2 gets exponentially smaller           and this one is a constant increase the only reason this doesnt cause jitters, is because theres no way to finish a frame with out the if statement below it running last

        // if(Mathf.Abs(targetFOV - thisCam.orthographicSize) <= 1)
        // {
        //     thisCam.orthographicSize = targetFOV;
        // }

    }
    
}
