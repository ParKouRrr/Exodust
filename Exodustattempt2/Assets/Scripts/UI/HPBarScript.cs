using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour
{
    public HealthSystem selectedHealthSystem;
    public Transform selectedTransform;
    public float yOffset;

    void Start()
    {
        selectedTransform = this.gameObject.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector2(selectedTransform.position.x, selectedTransform.position.y + yOffset);
    }

    public void ChangeSelectedObjects(Transform transformToSelect, float objectYOffset)
    {
        selectedTransform = transformToSelect;
        yOffset = objectYOffset;
        selectedHealthSystem = selectedTransform.gameObject.GetComponent<HealthSystem>();
    }    
    public void UpdateValues(float percentOfHP)
    {   
        percentOfHP = Mathf.Clamp(percentOfHP, 0.0f, 1.0f);
        transform.localScale = new Vector2(percentOfHP, transform.localScale.y);
    }
}
