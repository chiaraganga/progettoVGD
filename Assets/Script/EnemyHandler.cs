using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2f;
    private float stoppingDistance = 2f;

    private float idleDistance = 10f;
    private float followRange = 9f;
    private float originalYPosition;

    public Health_manager healthManager;
    private bool isDying = false;

        public bool IsAttacking { get; private set; } // Variabile booleana che indica se il nemico sta attaccando
        // Aggiungi una variabile per la durata dell'attacco
    private float attackDuration = 1f; // Imposta questo al tempo che desideri
    private float attackTimer = 0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null || animator == null || player == null)
        {
            Debug.LogError("Manca un componente essenziale o il giocatore non è stato assegnato.");
            return;
        }

        agent.stoppingDistance = stoppingDistance;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        originalYPosition = transform.position.y;
    }

    void Update()
    {
        // Aggiungi il tempo trascorso dall'ultimo frame al timer di attacco
        if (IsAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                IsAttacking = false;
                attackTimer = 0f;
            }
        }
        
        if (player != null)
        {
            if (healthManager.death && !isDying)
            {
                StartDeath();
            }
            else if (!healthManager.death && !isDying)
            {
                HandleMovementAndAttacking();
            }
        }
        
        if (!isDying)
        {
            transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);
        }
    }

    void HandleMovementAndAttacking()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        animator.SetBool("isIdle", false);
        if (distanceToPlayer <= attackDistance)
        {
            StartAttack();
        }
        else if (distanceToPlayer <= followRange)
        {
            StartChase();
        }
        else if (distanceToPlayer > idleDistance)
        {
            StartIdle();
        }

        if (agent.velocity.magnitude > 0 && !isDying)
        {
            LookAtDirection(agent.velocity.normalized);
        }
    }
void StartAttack()
{
    IsAttacking = true;
    attackTimer = 0f; // Resetta il timer di attacco a ogni inizio di attacco
    
    agent.isStopped = true;
    animator.SetBool("attack", true);
    animator.SetFloat("Velocity", 0f);
    LookAtPlayer();
}



    void StartChase()
    {
        IsAttacking = false; // Impostare IsAttacking a false quando inizia l'inseguimento

        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
    }

    void StartIdle()
    {
        IsAttacking = false; // Impostare IsAttacking a false quando inizia l'ozio

        agent.isStopped = true;
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", 0f);
        animator.SetBool("isIdle", true);
    }

    void StartDeath()
    {
        IsAttacking = false; // Impostare IsAttacking a false quando inizia la morte

        isDying = true;
        agent.SetDestination(transform.position);
        animator.SetBool("attack", false);  
        animator.SetFloat("Velocity", 0);
        animator.SetBool("death", true);  
        healthManager.death = true; 
    }

    void LookAtDirection(Vector3 direction)
    {
        if (!healthManager.death && !isDying)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void LookAtPlayer()
    {
        if (!healthManager.death && !isDying)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
