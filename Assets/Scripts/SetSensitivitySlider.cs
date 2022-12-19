using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSensitivitySlider : MonoBehaviour
{
    [SerializeField] Slider sS;
    void Awake() 
    {
        sS.value = PlayerPrefs.GetFloat("sensitivity"); 
        
    }
}
