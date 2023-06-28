using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player, enemy; // Assegna qui il tuo giocatore.
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 1.5f; // Definisci la distanza a cui il nemico deve essere per attaccare. Modifica questo valore se necessario.
    private float idleDistance = 10f; // Definisci la distanza a cui il nemico entra nello stato di riposo. Modifica questo valore se necessario.

    private float lookAtWeight = 1.0f;

    private bool isChasing = false;

    void Start()
    {
        // Assicurati che il GameObject abbia un NavMeshAgent e un Animator.
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null || animator == null || player == null)
        {
            Debug.LogError("Componente critico mancante dal nemico o giocatore non assegnato.");
            return;
        }

        agent.stoppingDistance = attackDistance;
    }

    void Update()
    {
        if (player != null)
        {
            // Calcola la distanza dal giocatore
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= idleDistance)
            {
                // Insegue il giocatore.
                agent.SetDestination(player.transform.position);
                isChasing = true;

                // Guarda il giocatore
                Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookAtWeight);
            }
            else if (isChasing)
            {
                // Entra nello stato di riposo.
                agent.SetDestination(transform.position);
                isChasing = false;
            }

            // Aggiorna i parametri dell'animatore
            animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante il movimento
            animator.SetBool("attack", isChasing && distanceToPlayer <= attackDistance); // Attacca quando è a distanza di attacco e insegue
            animator.SetFloat("Velocity", agent.velocity.magnitude); // Utilizza la velocità dell'agente
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            agent.SetDestination(transform.position);
            isChasing = false;
        }
    }
}
