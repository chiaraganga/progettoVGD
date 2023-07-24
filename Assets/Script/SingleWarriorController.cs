using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SingleWarriorController : MonoBehaviour
{
    public GameObject player; // Assign your player here.
    public Health_manager healthManager; // Assign your Health_manager here.
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f; // Define how close the enemy needs to be to attack. Modify this value as needed.
    private GameObject ares;

    void Start()
    {
        gameObject.SetActive(true);
        
        // Ensure the GameObject has a NavMeshAgent and Animator.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null || animator == null || player == null || healthManager == null)
        {
            Debug.LogError("Critical component missing from enemy or player not assigned.");
            return;
        }
    }

    void Update()
    {
        if (player != null && !healthManager.death) // Check if the enemy is not dead
        {
            agent.transform.LookAt(player.transform.position);
            // Calculate distance to player
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
        else if (player != null && healthManager.death)
        {
            
                agent.SetDestination(transform.position);

                // Update animator parameters
                animator.SetBool("grounded", true); // Assuming the enemy is always on the ground while moving
                animator.SetBool("attack", false); // Not attacking while moving
                animator.SetFloat("Velocity", agent.velocity.magnitude); // Use the agent's velocity
        }

    }
}
