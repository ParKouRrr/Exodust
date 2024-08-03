using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    //next: implement the time pool so it actually works
    public float maxTimeSlowPool = 100;
    public float currentTimeSlowPool = 100;// Essay time! :D
    public Vector3 valueCache;
    public bool isTimerRunning;
    public int I = 0;
    //The amount and duration of time slow depends on this pool, or, float value. 
    //Whenever the slowTime function is called, the duration and intensity
    //of the time slow is multiplied increasingly more by the remaining "Time" in the
    //"pool." This is to create a psuedo-cooldown between significant (and minor) time slows,
    //which could slow the pace of the game and become annoying
    //Notes: Just kidding, like I would take those read the code yourself
    public void slowTime(float duration, float scaleAmount, float allowance) //1 slowAmount = 100% speed
    {
        if(isTimerRunning)
        {
            if(scaleAmount < valueCache.y)
            {   
                StopCoroutine("Invoke_RealTimeTho");
                StartCoroutine(Invoke_RealTimeTho(duration)); 
                Time.timeScale = scaleAmount * (currentTimeSlowPool/maxTimeSlowPool);
            }
            else
            {
                valueCache.y += scaleAmount;
                valueCache.x = Mathf.Clamp(duration + valueCache.y, 0, 3);
                valueCache.z += allowance;
            }
            return;
        }
        //simplify if performance becomes issue
        duration = duration;// * ((Mathf.Clamp(currentTimeSlowPool, 0, allowance)/maxTimeSlowPool)
        //+ (maxTimeSlowPool - Mathf.Clamp(currentTimeSlowPool, 0, allowance)) / 150); //the second part is just there to keep the time slow dropoff from being too opressive
        scaleAmount = scaleAmount * (currentTimeSlowPool/maxTimeSlowPool);
        StartCoroutine(Invoke_RealTimeTho(duration));      
        Time.timeScale = scaleAmount;
        valueCache.y += scaleAmount;
        valueCache.x = Mathf.Clamp(duration + valueCache.y, 0, 3);
        valueCache.z += allowance;
    }
    public void unslowTime()
    {
        Time.timeScale = 1f;
        if(valueCache.z != 0)
        {
            slowTime(valueCache.x, valueCache.y, valueCache.z);
        }
        valueCache = new Vector3(0, 0, 0);
    }
    private IEnumerator Invoke_RealTimeTho(float seconds)
    {
        isTimerRunning = true;
        yield return new WaitForSecondsRealtime(seconds);
        isTimerRunning = false;
        unslowTime();

    }
}
