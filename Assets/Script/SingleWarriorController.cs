using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SingleWarriorController : MonoBehaviour
{
    public GameObject player;
    public Health_manager healthManager;
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 1.5f;
    public bool IsAttacking { get; private set; } // Aggiunto il campo IsAttacking

    void Start()
    {
        gameObject.SetActive(true);
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
        if (player != null && !healthManager.death)
        {
            agent.transform.LookAt(player.transform.position);
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            
            if (distanceToPlayer > attackDistance)
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("grounded", true);
                animator.SetBool("attack", false);
                animator.SetFloat("Velocity", agent.velocity.magnitude);
                IsAttacking = false; // Aggiunto il controllo IsAttacking
            }
            else
    {
        agent.transform.LookAt(player.transform.position);
        agent.SetDestination(transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0);
        IsAttacking = true;
        //Debug.Log("Enemy is attacking"); // Aggiunto il debug log
    }
        }
         else if (player != null && healthManager.death)
    {
        agent.SetDestination(transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        IsAttacking = false;
        //Debug.Log("Enemy is not attacking"); // Aggiunto il debug log
    }
    }
   

}
