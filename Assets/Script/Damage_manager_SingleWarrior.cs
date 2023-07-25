using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_manager_SingleWarrior : MonoBehaviour
{
    public int damage_done = 1;
    public Transform attack_point;
    public float attack_range;
    public LayerMask enemy_layer;

    public SingleWarriorController singleWarriorController; // Reso pubblico per impostarlo dall'editor di Unity

    void Update()
{
    if (this.CompareTag("Enemy") && IsEntityAttacking()) 
    {
        detect_enemy_attack();
    }
}


    private bool IsEntityAttacking()
    {
        if (singleWarriorController != null)
        {
            return singleWarriorController.IsAttacking;
        }
        return false;
    }

private void detect_enemy_attack()
{
    if (singleWarriorController != null && singleWarriorController.IsAttacking)
    {
        Collider[] hit_players = Physics.OverlapSphere(attack_point.position, attack_range);

        foreach (Collider player in hit_players)
        {
            if (player.CompareTag("Player"))
            {
                Health_manager healthManager = player.GetComponent<Health_manager>();
                if (healthManager != null)
                {
                    healthManager.Damages(damage_done);
                }
                else
                {
                    Debug.LogError("Player does not have a Health_manager component!");
                }
            }
        }
    }
}


    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }
}
