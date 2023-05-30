using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health_manager : MonoBehaviour
{

    public health_bar barra_vita;
    public int health;
    public int max_health=15;

    public void Start()
    {
        health = max_health;
        barra_vita.Set_max_health(max_health);

        //respawn_point = Player.transform.position;
    }

    public void Update()
    {
       
    }

    public void Damages(int damage)
    {
        health -= damage;
        barra_vita.Set_health(health);
    }

    public void Healing(int heal)
    {
        health += heal;
        if (health > max_health)
        {
            health = max_health;
        }
    }
}
