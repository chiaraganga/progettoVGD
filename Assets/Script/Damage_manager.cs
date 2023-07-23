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

    public EnemyHandler enemyHandler; // Reso pubblico per impostarlo dall'editor di Unity

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !enemyHandler.IsAttacking) // Utilizza la variabile IsAttacking del nemico
        {
            Invoke("detect_player_attack", 0.5f);
        }
        else if ((this.CompareTag("Enemy") || this.CompareTag("Ares")) && enemyHandler.IsAttacking) // Utilizza la variabile IsAttacking del nemico
        {
            Invoke("detect_enemy_attack", 0.5f);
        }
    }

    private void detect_player_attack()
    {
        Collider[] hit_enemies = Physics.OverlapSphere(attack_point.position, attack_range, enemy_layer);

        foreach (Collider enemy in hit_enemies)
        {
            enemy.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

    private void detect_enemy_attack()
    {
        if (!enemyHandler.IsAttacking) // Utilizza la variabile IsAttacking del nemico
        {
            return;
        }

        Collider[] hit_players = Physics.OverlapSphere(attack_point.position, attack_range, player_layer);

        foreach (Collider player in hit_players)
        {
            player.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }
}
