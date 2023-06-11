using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player, enemy; // Assign your player here.
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f; // Define how close the enemy needs to be to attack. Modify this value as needed.

    void Start()
    {
        // Ensure the GameObject has a NavMeshAgent and Animator.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null && animator == null && player == null)
        {
            Debug.LogError("Critical component missing from enemy or player not assigned.");
            return;
        }
    }

    void Update()
    {
        if (player != null)
        {
            agent.transform.LookAt(player.transform.position);
            // Calculate distance to player
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > attackDistance)
            {
                // Chase the player.
                agent.SetDestination(player.transform.position);

                // Update animator parameters
                animator.SetBool("grounded", true); // Assuming the enemy is always on the ground while moving
                animator.SetBool("attack", false); // Not attacking while moving
                animator.SetFloat("Velocity", agent.velocity.magnitude); // Use the agent's velocity
            }
            else
            {
                agent.transform.LookAt(player.transform.position);
                agent.SetDestination(transform.position);
                // Attack the player
                animator.SetBool("grounded", true); // Assuming the enemy is always on the ground while attacking
                animator.SetBool("attack", true);
                animator.SetFloat("Velocity", 0); // No velocity while attacking
            }
        }
    }
    void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "player")
        {
            agent.transform.LookAt(player.transform.position);
        }
    }
}