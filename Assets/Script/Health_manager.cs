using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_manager : MonoBehaviour
{
    
    public int health;
    public int max_health;
    public float invicibility_max_time;
    public float invincibility_actual_time;




    public void Start()
    {
        health = max_health;



    }

    public void Update()
    {

    }

    public void Damages(int damage)
    {
        health -= damage;
    }
}