using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public RectTransform transformSettings;
    public float ResSettingsX;
    public float ResSettingsY;

    // Start is called before the first frame update
    void Awake()
    {
        transformSettings.sizeDelta = new Vector2 (ResSettingsX, ResSettingsY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
