using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_manager : MonoBehaviour
{
    public int damage_done = 1;
    public Transform attack_point;
    public float attack_range;
    public LayerMask enemy_layer;

    public Player_controller playerController; // Reso pubblico per impostarlo dall'editor di Unity

    void Update()
{
    if (playerController.isAttacking) 
    {
        detect_player_attack();
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

    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }


}
