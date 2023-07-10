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
    private bool isAttacking = false;
    private bool isPerformingAttack = false;

    private float idleDistance = 10f;
    private float followRange = 9f;
    private float originalYPosition;

    public Health_manager healthManager;

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
        if (player != null && healthManager.death)
        {
            StartDeath();
        }
        else if (player != null && !healthManager.death)
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
                if (distanceToPlayer <= attackDistance && !isPerformingAttack)
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

        transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);
    }

    void StartAttack()
    {
        if (healthManager.death)
            return;

        Debug.Log("Inizio attacco.");
        isAttacking = true;
        StartCoroutine(PerformAttack());
    }

    void StopAttack()
    {
        if (healthManager.death)
            return;

        Debug.Log("Fermo attacco.");
        isAttacking = false;
        isPerformingAttack = false;
        agent.velocity = Vector3.zero;
        animator.SetBool("attack", false);
        agent.updatePosition = true;
    }

    void LookAtDirection(Vector3 direction)
    {
        if (!isAttacking && !healthManager.death)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void StartChase()
    {
        if (healthManager.death)
            return;

        Debug.Log("Inizio inseguimento.");
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        agent.updateRotation = false;
    }

    void StartIdle()
    {
        if (healthManager.death)
            return;

        Debug.Log("Inizio stato idle.");
        agent.isStopped = true;
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", 0f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        agent.updateRotation = false;
    }

    void StartDeath()
    {
        Debug.Log("Inizio morte.");
        isAttacking = false;
        agent.SetDestination(transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", 0);
        animator.SetBool("death", true);
    }

    void LookAtPlayer()
    {
        if (!healthManager.death)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Coroutine per eseguire l'attacco
    // Coroutine per eseguire l'attacco
    // Coroutine per eseguire l'attacco
    IEnumerator PerformAttack()
{
    if(!healthManager.death)
    {
        Debug.Log("Eseguo attacco.");
        isPerformingAttack = true;  // Impostiamo il flag di attacco
        agent.isStopped = true;  // Fermiamo l'agente
        agent.updatePosition = false;  // Impediamo all'agente di aggiornare automaticamente la sua posizione

        animator.SetBool("grounded", true);  // Impostiamo l'animazione a terra
        animator.SetBool("attack", true);  // Impostiamo l'animazione di attacco a true
        animator.SetFloat("Velocity", 0f);  // Impostiamo la velocità dell'animazione a 0

        // Calcoliamo la distanza dal nemico al giocatore
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Se la distanza al giocatore è minore o uguale alla distanza di attacco
            if (distanceToPlayer <= attackDistance)
            {
                // Aspettiamo la durata dell'animazione di attacco più un po' di tempo extra
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + .9f);
                agent.isStopped = false;  // Facciamo ripartire l'agente

                StopAttack(); // Interrompiamo l'attacco dopo che l'animazione è completata

                yield return new WaitForSeconds(0.5f); // Diamo un po' di tempo prima di un altro attacco
            }

        isPerformingAttack = false;  // Resettiamo il flag di attacco
    }
    else if (healthManager.death)
        yield break;
    
}

}
