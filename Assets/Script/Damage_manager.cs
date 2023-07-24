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

    public SingleWarriorController singleWarriorController; // Reso pubblico per impostarlo dall'editor di Unity

    public EnemyAresController enemyAresController; // Reso pubblico per impostarlo dall'editor di Unity


    void Update()
{
    if ((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5)) && !IsEntityAttacking()) 
    {
        Invoke("detect_player_attack", 0.5f);
    }
    else if ((this.CompareTag("Enemy") || this.CompareTag("Ares")) && IsEntityAttacking()) 
    {
        Invoke("detect_enemy_attack", 0.5f);
    }
}


    private bool IsEntityAttacking()
{
    Debug.Log("IsEntityAttacking called");
    // Check if it's an enemy or a single warrior and return the correct IsAttacking value
    if (enemyHandler != null)
        return enemyHandler.IsAttacking;
    else if (singleWarriorController != null)
    {
        Debug.Log("SingleWarriorController IsAttacking: " + singleWarriorController.IsAttacking); // Aggiunto il debug log
        return singleWarriorController.IsAttacking;
    }
    else if (enemyAresController != null)
    {
        Debug.Log("EnemyAresController IsAttacking: " + enemyAresController.isAttacking);
        return enemyAresController.isAttacking;
    }

    return false;
}

    private void detect_player_attack()
    {
        Debug.Log("detect_enemy_attack called");
        Collider[] hit_enemies = Physics.OverlapSphere(attack_point.position, attack_range, enemy_layer);

        foreach (Collider enemy in hit_enemies)
        {
            enemy.GetComponent<Health_manager>().Damages(damage_done);
        }
    }

        private void detect_enemy_attack()
        {
            bool isAttacking = false;

            if (enemyHandler != null)
            {
                isAttacking = enemyHandler.IsAttacking;
            }
            else if (singleWarriorController != null)
            {
                isAttacking = singleWarriorController.IsAttacking;
            }
            else if (enemyAresController != null)
            {
                isAttacking = enemyAresController.isAttacking;
            }

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




    private void OnDrawGizmosSelected()
    {
        if (attack_point == null)
            return;
        Gizmos.DrawSphere(attack_point.position, attack_range);
    }
}
