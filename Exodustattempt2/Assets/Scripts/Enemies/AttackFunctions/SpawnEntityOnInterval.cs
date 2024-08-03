using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEntityOnInterval : MonoBehaviour
{
    public float interval; //Cooldown between burstsn seconds
    public float burstInterval = 0.2f; //The rate of summons during a burst
    public int burstAmount = 3; //Amount of summons per burst
    public GameObject entityToSummon;
    public Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        CancelInvoke("StartBurst");
        Invoke("StartBurst", interval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Vector3 offset, GameObject entity
    public void StartBurst()
    {
        for(int i = 0; i < burstAmount; i++)
        {
            Invoke("SummonEntity", i * burstInterval);
        }
        Invoke("StartBurst", interval);
    }
    public void SummonEntity()
    {
        Instantiate(entityToSummon, transform.position + offset, Quaternion.identity);
    }
}
