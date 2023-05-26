using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_manager : MonoBehaviour
{

   
    public int health;
    public int max_health;
    public float invicibility_max_time;
    public float invincibility_actual_time;
   
    public float respawn_delay;

    public void Start()
    {
        health = max_health;
       
        //respawn_point = Player.transform.position;

    }

    public void Update()
    {
        if (invincibility_actual_time > 0)
        {
            invincibility_actual_time -= Time.deltaTime;
        }
    }

    public void Damages(int damage)
    {





        if (invincibility_actual_time <= 0)
        {
            health -= damage;

            
            invincibility_actual_time = invicibility_max_time;


        }







    }

    public void Healing(int heal)
    {
        health += heal;
        if (health > max_health)
        {
            health = max_health;
        }
    }
    public void Respawn()
    {


    }

}
