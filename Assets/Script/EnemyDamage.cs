using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage_done = 1;
    public Transform attack_point;
    public float attack_range;
    public LayerMask enemy_layer;

    public EnemyHandler enemyHandler; // Reso pubblico per impostarlo dall'editor di Unity

    void Update()
    {
        if (enemyHandler.IsAttacking) 
        {
            detect_enemy_attack();
        }
    }

    private void detect_enemy_attack()
    {
        Collider[] hit_enemies = Physics.OverlapSphere(attack_point.position, attack_range, enemy_layer);

        foreach (Collider enemy in hit_enemies)
        {
            enemy.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }
}
