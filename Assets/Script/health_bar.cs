using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health_bar : MonoBehaviour
{
    public Slider slider;
    
    public void  Set_max_health(int health)
    {
       
            slider.maxValue = health;
            slider.value = health;
        
    }
    public void Set_health(int health)
    {
        
        slider.value = health;
    }
}
