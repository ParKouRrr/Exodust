using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTracker_DEV : MonoBehaviour
{
    public float horInput;
    public float verInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        verInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");
    }
}
