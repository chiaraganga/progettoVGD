using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player; // Assegna qui il tuo giocatore.
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f; // Definisci la distanza a cui il nemico deve essere per attaccare. Modifica questo valore se necessario.
    private float stoppingDistance = 3f; // Definisci la distanza a cui il nemico si ferma per attaccare. Modifica questo valore se necessario.
    private bool isAttacking = false; // Stato di attacco del nemico
    private float idleDistance = 10f; // Definisci la distanza a cui il nemico entra in stato di riposo. Modifica questo valore se necessario.

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
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // Disabilita l'evitamento degli ostacoli
    }

    void Update()
{
    if (player != null)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer > stoppingDistance)
        {
            if (!isAttacking && distanceToPlayer > idleDistance)
            {
                agent.enabled = false;
                animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante l'Idle
                animator.SetBool("attack", false); // Non attacca durante l'Idle
                animator.SetFloat("Velocity", 0f); // Nessuna velocità durante l'Idle

                // Blocca la rotazione sull'asse X e Z
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            }
            else
            {
                agent.enabled = true;
                agent.SetDestination(player.transform.position);

                animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante il movimento
                animator.SetBool("attack", false); // Non attacca mentre si muove
                animator.SetFloat("Velocity", agent.velocity.magnitude); // Utilizza la velocità dell'agente
            }
        }
        else
        {
            if (!isAttacking)
            {
                StartCoroutine(PerformAttack());
            }
        }
    }
    else
    {
        isAttacking = false;
    }
}

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        agent.isStopped = true;

        animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante l'attacco
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0f); // Nessuna velocità durante l'attacco

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + .5f);

        agent.isStopped = false;

        isAttacking = false;
    }
}

