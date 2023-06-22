using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Health_manager : MonoBehaviour
{
    private Animator anim;
    public health_bar barra_vita;
    public int health;
    public int max_health=15;
    public bool dead=false;
    private GameObject Ares;
    
    
    public void Start()
    {
        anim = GetComponent<Animator>();
        Ares = GameObject.FindGameObjectWithTag("Ares");
        health = max_health;
        barra_vita.Set_max_health(max_health);
        
        //respawn_point = Player.transform.position;
    }

    public void Update()
    {
        if(this.CompareTag("Player"))
        {
            if (dead == true)
            {
                dead = false;
                anim.SetBool("dead", false);
            }
            dead = false;
            if (health == 0)
            {
                dead = true;
                anim.SetBool("dead", true);
                Invoke("destroy_anim", 4.4f);
            }
        }
    }
   

    public void Damages(int damage)
    {
        health -= damage;
        barra_vita.Set_health(health);
        if(this.CompareTag("Enemy"))
        {
            if (health==0)
            {
                Ares.SetActive(true);
                this.gameObject.SetActive(false);
            }
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
    public void destroy_anim()
    {
        barra_vita.Set_max_health(0);
        SceneManager.LoadScene(6);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Player"))
        {
            if (other.CompareTag("Collect"))
            {
                Healing(10);
            }
        }
        
    }
}
