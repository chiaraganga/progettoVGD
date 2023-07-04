using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_manager : MonoBehaviour
{
    public int damage_done = 1;
    public Transform attack_point;
    public float attack_range;
    public LayerMask enemy_layer;
    public LayerMask player_layer;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {




        
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("detect_player_attack", 0.5f); //invoca la funzione con un ritardo di 0,7 secondi
        }
        if(this.CompareTag("Enemy") || this.CompareTag("Ares"))
        {
            Invoke("detect_enemy_attack", 0.5f);
        }

    }
    private void detect_player_attack()
    {
        Collider[] hit_enemies = Physics.OverlapSphere(attack_point.position, attack_range, enemy_layer);// array di memici che contiene tutto ciò che viene toccato dalla punta della spada in un area sferica di raggio variabile

        foreach (Collider enemy in hit_enemies)
        {
            enemy.GetComponent<Health_manager>().Damages(damage_done);
            //Debug.Log("hit" + enemy.name);
        }

    }
    private void detect_enemy_attack()
    {
        Collider[] hit_players = Physics.OverlapSphere(attack_point.position, attack_range, player_layer);// array di memici che contiene tutto ciò che viene toccato dalla punta della spada in un area sferica di raggio variabile

        foreach (Collider player in hit_players)
        {
            player.GetComponent<Health_manager>().Damages(damage_done);
            //Debug.Log("hit" + player.name);
        }

    }
    private void OnDrawGizmosSelected()//disegna la sfera in modo da poterla settare da inspector in modo corretto
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position,attack_range);
    }



}
