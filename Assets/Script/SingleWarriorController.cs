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
    public bool IsAttacking { get; private set; }

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
        Health_manager.enemiesDead = 0 ;
    }

    void Update()
    {
        if (player != null && !healthManager.death)
        {
            //agent.transform.LookAt(player.transform.position);
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            // Fai guardare Ares a questa nuova destinazione
            agent.transform.LookAt(targetPosition);

            
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            
            if (distanceToPlayer > attackDistance)
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("grounded", true);
                animator.SetBool("attack", false);
                animator.SetFloat("Velocity", agent.velocity.magnitude);
                IsAttacking = false;
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
