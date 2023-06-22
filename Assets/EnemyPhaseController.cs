using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPhaseController : MonoBehaviour
{
    public GameObject player; // Assegna qui il tuo personaggio giocatore.
    private NavMeshAgent agent;
    private Animator animator;
    private float attackDistance = 2.5f; // Definisci a che distanza il nemico deve essere per attaccare. Modifica questo valore se necessario.
    private bool isPhase1 = true;
    public GameObject cubeEnemy;
    public GameObject aresEnemy;
    private bool cubeHit = false;

    public bool startPhaseDue = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null || animator == null || player == null)
        {
            Debug.LogError("Componenti critici mancanti o personaggio giocatore non assegnato.");
            return;
        }
    }

    void Update()
    {
        if (startPhaseDue)
        {
            StartPhase2();
        }

        if (player != null)
        {
            if (isPhase1)
            {
                agent.transform.LookAt(player.transform.position);
                float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                if (distanceToPlayer > attackDistance)
                {
                    agent.SetDestination(player.transform.position);
                    animator.SetBool("grounded", true);
                    animator.SetBool("attack", false);
                    animator.SetFloat("Velocity", agent.velocity.magnitude);
                }
                else
                {
                    agent.transform.LookAt(player.transform.position);
                    agent.SetDestination(transform.position);
                    animator.SetBool("grounded", true);
                    animator.SetBool("attack", true);
                    animator.SetFloat("Velocity", 0);
                }
            }
            else
            {
                agent.transform.LookAt(player.transform.position);
                float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                if (distanceToPlayer > attackDistance)
                {
                    agent.SetDestination(player.transform.position);
                    animator.SetBool("grounded", true);
                    animator.SetBool("attack", false);
                    animator.SetFloat("Velocity", agent.velocity.magnitude);
                }
                else
                {
                    agent.transform.LookAt(player.transform.position);
                    agent.SetDestination(transform.position);
                    animator.SetBool("grounded", true);
                    animator.SetBool("attack", true);
                    animator.SetFloat("Velocity", 0);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject == cubeEnemy)
    {
        cubeHit = true;
        StartPhase2(); // Avvia direttamente la fase 2
    }
}

private void StartPhase2()
{
    if (cubeHit)
    {
        isPhase1 = false;
        cubeEnemy.SetActive(false);
        aresEnemy.SetActive(true);
        // Aggiungi qui eventuali cambiamenti necessari per la fase 2 di Ares
    }
}

}
