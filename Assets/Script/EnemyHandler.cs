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
    private float stoppingDistance = 2.5f; // Definisci la distanza a cui il nemico si ferma per attaccare. Modifica questo valore se necessario.
    private bool isAttacking = false; // Stato di attacco del nemico
    private bool isPerformingAttack = false; // Stato di esecuzione dell'attacco del nemico

    private float idleDistance = 10f; // Definisci la distanza a cui il nemico entra in stato di riposo. Modifica questo valore se necessario.
    private float followRange = 9f; // Definisci la distanza a cui il nemico inizia a seguire il giocatore. Modifica questo valore se necessario.

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

            if (isAttacking)
            {
                // Controlla la distanza solo durante l'attacco
                if (distanceToPlayer > attackDistance)
                {
                    // Il giocatore è troppo lontano, smetti di attaccare
                    StopAttack();
                }
            }
            else
            {
                // Nemico non in attacco

                if (distanceToPlayer <= attackDistance)
                {
                    // Il giocatore è nel range di attacco, inizia l'attacco
                    StartAttack();
                }
                else if (distanceToPlayer <= followRange)
                {
                    // Il giocatore è nel range di inseguimento
                    StartChase();
                }
                else if (distanceToPlayer > idleDistance)
                {
                    // Il giocatore è fuori dal range di inseguimento, resta in idle
                    StartIdle();
                }
            }

            if (!isPerformingAttack && agent.velocity.magnitude > 0)
            {
                // Il nemico si sta muovendo, guarda verso la direzione del movimento
                LookAtDirection(agent.velocity.normalized);
            }
            else if (isAttacking)
            {
                // Il nemico è in attacco, guarda verso se stesso
                LookAtSelf();
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        isPerformingAttack = true;
        agent.velocity = Vector3.zero; // Imposta la velocità dell'agente a zero per fermarlo

        // Salva la posizione originale del nemico sull'asse Y
        float originalYPosition = transform.position.y;

        animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante l'attacco
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0f); // Nessuna velocità durante l'attacco

        // Imposta la posizione corrente del nemico sulla posizione originale sull'asse Y
        transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);
    }

    void StopAttack()
    {
        isAttacking = false;
        isPerformingAttack = false;
        agent.velocity = Vector3.zero; // Imposta la velocità dell'agente a zero per fermarlo
        animator.SetBool("attack", false);
    }

    void LookAtSelf()
    {
        Quaternion lookRotation = Quaternion.LookRotation(transform.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void LookAtDirection(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void StartChase()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        agent.updateRotation = false;
    }

    void StartIdle()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", 0f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        agent.updateRotation = false;
    }

    IEnumerator PerformAttack()
    {
        isPerformingAttack = true;

        agent.isStopped = true;
        agent.updatePosition = false; // Disabilita l'aggiornamento della posizione dell'agente di navigazione

        // Salva la posizione originale del nemico sull'asse Y
        float originalYPosition = transform.position.y;

        animator.SetBool("grounded", true); // Si assume che il nemico sia sempre a terra durante l'attacco
        animator.SetBool("attack", true);
        animator.SetFloat("Velocity", 0f); // Nessuna velocità durante l'attacco

        // Imposta la posizione corrente del nemico sulla posizione originale sull'asse Y
        transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + .9f);

        agent.isStopped = false;
        agent.updatePosition = true; // Abilita nuovamente l'aggiornamento della posizione dell'agente di navigazione

        isPerformingAttack = false;

        // Aggiungi un tempo di attesa tra un attacco e l'altro
        float attackCooldown = 2.0f; // Tempo di attesa desiderato (puoi modificare questo valore)
        yield return new WaitForSeconds(attackCooldown);

        // Riprendi l'attacco solo se il giocatore è ancora presente
        if (player != null)
        {
            StartCoroutine(PerformAttack());
        }
    }

}
