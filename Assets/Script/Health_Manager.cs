using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Manager : MonoBehaviour
{
    public Player_controller Player;
    public int health;
    public int max_health;
    public float invicibility_max_time;
    public float invincibility_actual_time;
    private bool alive;
    private Vector3 respawn_point;
    public float respawn_delay;

    public void Start()
    {
        health = max_health;
        Player = FindObjectOfType<Player_controller>();
        //respawn_point = Player.transform.position;

    }

    public void Update()
    {
        if (invincibility_actual_time > 0)
        {
            invincibility_actual_time -= Time.deltaTime;
        }
    }

    public void Damages(int damage, Vector3 direction)
    {





        if (invincibility_actual_time <= 0)
        {
            health -= damage;

            Player.KnockBack(direction);
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

