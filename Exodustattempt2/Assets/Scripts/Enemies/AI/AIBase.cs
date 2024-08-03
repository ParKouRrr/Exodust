using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{                                                 //Fleeing       //Fleeing while attacking      //Backing up   //Neutral      //Alert       //Slow Approach while using ranged  //GET EM
    public int aggroState; //                      -3 D:>         -2 <:(                         -1 :(          0 :|           1 >:|          2 >:(                               3 D:<
    public float aggression = 1f;   //out of 1;
    public GameObject aggroHitbox; //Declared in editor
    public Transform aggroedTransform;
    public AggroHitboxScript aggroBoxScript;
    public string[] attackFunctions; //Declared in editor

    void Awake()
    {
        aggroBoxScript = aggroHitbox.GetComponent<AggroHitboxScript>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeAttack(string functionName, float delay)
    {
        Invoke(functionName, delay);
    }

    public void SetAggressionState(int a)
    {
        aggroState = a;
    }

    public void SetAggroedTransform(Transform t_transform)
    {
        aggroedTransform = t_transform;
    }
}
