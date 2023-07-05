using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f;
    private float stoppingDistance = 2.5f;
    private bool isAttacking = false;
    private bool isPerformingAttack = false;

    private float idleDistance = 10f;
    private float followRange = 9f;

    private float originalYPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null || animator == null || player == null)
        {
            Debug.LogError("Componenti critici mancanti dal nemico o il giocatore non è stato assegnato.");
            return;
        }

        agent.stoppingDistance = stoppingDistance;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        originalYPosition = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (isAttacking)
            {
                if (distanceToPlayer > attackDistance)
                {
                    StopAttack();
                }
            }
            else
            {
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
            }

            if (!isPerformingAttack && agent.velocity.magnitude > 0)
            {
                LookAtDirection(agent.velocity.normalized);
            }
            else if (isAttacking)
            {
                LookAtPlayer();
            }
        }
        else
        {
            isAttacking = false;
        }

        transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);
    }

    void StartAttack()
    {
        isAttacking = true;
        isPerformingAttack = true;
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.velocity = Vector3.zero;

        animator.SetBool("grounded", true);
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0f);
    }

    void StopAttack()
    {
        isAttacking = false;
        isPerformingAttack = false;
        agent.velocity = Vector3.zero;
        animator.SetBool("attack", false);
    }

    void LookAtDirection(Vector3 direction)
    {
        if (!isAttacking)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void StartChase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        agent.updateRotation = false;
    }

    void StartIdle()
    {
        agent.isStopped = true;
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", 0f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        agent.updateRotation = false;
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    IEnumerator PerformAttack()
    {
        isPerformingAttack = true;
        agent.isStopped = true;
        agent.updatePosition = false;

        animator.SetBool("grounded", true);
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0f);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + .9f);

        agent.isStopped = false;
        agent.updatePosition = true;

        isPerformingAttack = false;

        float attackCooldown = 2.0f;
        yield return new WaitForSeconds(attackCooldown);

        if (player != null)
        {
            StartCoroutine(PerformAttack());
        }
    }
}
