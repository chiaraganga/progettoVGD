using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresDamage : MonoBehaviour
{
    public int damage_done = 1;
    public Transform attack_point;
    public float attack_range;
    public LayerMask player_layer; // Ares attacca solo il player

    public EnemyAresController enemyAresController; // Reso pubblico per impostarlo dall'editor di Unity

    void Update()
    {
        if ((this.CompareTag("Ares")) && enemyAresController.isAttacking) 
        {
            detect_enemy_attack();
        }
    }

    private void detect_enemy_attack()
    {
        if (!enemyAresController.isAttacking)
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
