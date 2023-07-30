using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAresController : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f; // distanza a cui il nemico deve essere per attaccare.
    private float stoppingDistance = 3f; // distanza a cui il nemico si ferma per attaccare.
    public bool isAttacking = false; // Stato di attacco del nemico
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public Health_manager healthManager;

    private bool AresDeath = false; 

    void Start()
{
    gameObject.SetActive(false);

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
        if (player != null && !healthManager.death)
        {
             // Crea una nuova destinazione che ha la stessa y della posizione attuale di Ares
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        // Fai guardare Ares a questa nuova destinazione
        agent.transform.LookAt(targetPosition);
            // Calcola la distanza dal giocatore
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > stoppingDistance)
            {
                // Insegue il giocatore se non sta attaccando.
                if (!isAttacking)
                {
                    agent.SetDestination(player.transform.position);
                }

                // Aggiorna i parametri dell'animator
                animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante il movimento
                animator.SetBool("attack", false); // Non attacca mentre si muove
                animator.SetFloat("Velocity", agent.velocity.magnitude); // Utilizza la velocità dell'agente
            }
            else
            {
                // Ferma il nemico e attacca il giocatore solo se non sta già attaccando
                if (!isAttacking)
                {
                    StartCoroutine(PerformAttack());
                }
            }
        }
        else if (player != null && healthManager.death)
        {
                isAttacking = false;
                agent.SetDestination(transform.position);

                animator.SetBool("grounded", true);
                animator.SetBool("attack", false);
                animator.SetFloat("Velocity", agent.velocity.magnitude);

                AresDeath = true;
        }
    }

    IEnumerator PerformAttack()
    {
        // Imposta lo stato di attacco su true
        isAttacking = true;

        // Ferma l'agente durante l'attacco
        agent.isStopped = true;

        // Aggiorna i parametri dell'animator per avviare l'animazione di attacco
        animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante l'attacco
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0); // Nessuna velocità durante l'attacco

        // Attendi la fine dell'animazione di attacco
          yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + .5f);

        // Riavvia l'agente dopo l'attacco
        agent.isStopped = false;

        // Ripristina lo stato di attacco su false
        isAttacking = false;
    }
}
