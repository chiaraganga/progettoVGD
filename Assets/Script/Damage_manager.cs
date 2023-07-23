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

    private bool isAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking || Input.GetKey(KeyCode.Joystick1Button5) && !isAttacking)
        {
            StartAttackAnimation();
            Invoke("detect_player_attack", 0.5f);
            Invoke("EndAttackAnimation", 1.0f);
        }
        else if ((this.CompareTag("Enemy") || this.CompareTag("Ares")) && !isAttacking)
        {
            StartAttackAnimation();
            Invoke("detect_enemy_attack", 0.5f);
            Invoke("EndAttackAnimation", 1.0f);
        }
    }

    private void detect_player_attack()
    {
        if (!isAttacking)
        {
            return;
        }

        Collider[] hit_enemies = Physics.OverlapSphere(attack_point.position, attack_range, enemy_layer);

        foreach (Collider enemy in hit_enemies)
        {
            enemy.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

    private void detect_enemy_attack()
    {
        if (!isAttacking)
        {
            return;
        }

        Collider[] hit_players = Physics.OverlapSphere(attack_point.position, attack_range, player_layer);

        foreach (Collider player in hit_players)
        {
            player.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

    public void StartAttackAnimation()
    {
        isAttacking = true;
    }

    public void EndAttackAnimation()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }
}
