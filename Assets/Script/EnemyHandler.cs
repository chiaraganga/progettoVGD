using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public GameObject player;  // Riferimento al giocatore che il nemico deve seguire e attaccare
    private NavMeshAgent agent;  // Riferimento al componente NavMeshAgent del nemico
    private Animator animator;  // Riferimento all'Animator del nemico
    private float attackDistance = 2f;  // Distanza alla quale il nemico può attaccare il giocatore
    private float stoppingDistance = 2f;  // Distanza alla quale il nemico deve fermarsi
    private bool isAttacking = false;  // Flag per sapere se il nemico sta attaccando
    private bool isPerformingAttack = false;  // Flag per sapere se il nemico sta eseguendo l'animazione di attacco

    private float idleDistance = 10f;  // Distanza alla quale il nemico torna in modalità idle
    private float followRange = 9f;  // Distanza entro la quale il nemico inizierà a seguire il giocatore

    private float originalYPosition;  // Posizione Y originale del nemico

    public Health_manager healthManager; // Assign your Health_manager here.


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  // Otteniamo il riferimento al NavMeshAgent
        animator = GetComponent<Animator>();  // Otteniamo il riferimento all'Animator

        // Controlliamo se ci sono componenti mancanti o se il giocatore non è stato assegnato
        if (agent == null || animator == null || player == null)
        {
            Debug.LogError("Manca un componente essenziale o il giocatore non è stato assegnato.");
            return;
        }

        // Impostiamo la distanza di arresto e il tipo di evitamento degli ostacoli per l'agente
        agent.stoppingDistance = stoppingDistance;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        // Memorizziamo la posizione Y originale
        originalYPosition = transform.position.y;
    }

    void Update()
    {
        // Controlliamo se il giocatore è stato assegnato
        if (player != null && !healthManager.death)
        {
            // Calcoliamo la distanza tra il giocatore e il nemico
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // Controlliamo se il nemico sta attaccando
            if (isAttacking)
            {
                // Se la distanza al giocatore è maggiore della distanza di attacco, smettiamo di attaccare
                if (distanceToPlayer > attackDistance)
                {
                    StopAttack();
                }
            }
            else //Se non stiamo attaccando fai questo...
            {
                // Se la distanza al giocatore è minore o uguale alla distanza di attacco, iniziamo ad attaccare
                if (distanceToPlayer <= attackDistance && !isPerformingAttack)
                {
                    StartAttack();
                }
                // Se la distanza al giocatore è minore o uguale alla distanza di inseguimento, iniziamo a inseguire
                else if (distanceToPlayer <= followRange)
                {
                    StartChase();
                }
                // Se la distanza al giocatore è maggiore della distanza idle, torniamo allo stato idle
                else if (distanceToPlayer > idleDistance)
                {
                    StartIdle();
                }
            }

            // Se il nemico non sta attaccando e si sta muovendo, lo facciamo guardare nella direzione in cui si sta muovendo
            if (!isPerformingAttack && agent.velocity.magnitude > 0)
            {
                LookAtDirection(agent.velocity.normalized);
            }
            // Se il nemico sta attaccando, gli facciamo guardare il giocatore
            else if (isAttacking)
            {
                LookAtPlayer();
            }
        }
        else if (player != null && healthManager.death) //se è morto
        {
                
                agent.SetDestination(transform.position);

                // Update animator parameters
                animator.SetBool("grounded", true); // Assuming the enemy is always on the ground while moving
                animator.SetBool("attack", false); // Not attacking while moving
                animator.SetFloat("Velocity", agent.velocity.magnitude); // Use the agent's velocity
        }

        // Fissiamo la posizione Y del nemico alla sua posizione originale per evitare movimenti indesiderati
        transform.position = new Vector3(transform.position.x, originalYPosition, transform.position.z);
    }
    

    // Metodo per iniziare l'attacco
void StartAttack()
{
    if (!isPerformingAttack)
    {
        Debug.Log("Inizio attacco.");
        isAttacking = true;
        StartCoroutine(PerformAttack());  // Avviamo la coroutine per eseguire l'attacco
    }
}


    // Metodo per interrompere l'attacco
    void StopAttack()
{
    Debug.Log("Fermo attacco.");
    isAttacking = false;
    isPerformingAttack = false;
    agent.velocity = Vector3.zero;  // Fermiamo l'agente
    animator.SetBool("attack", false);  // Impostiamo l'animazione di attacco a false
    agent.updatePosition = true;  // Permettiamo all'agente di aggiornare automaticamente la sua posizione
}

    // Metodo per far guardare il nemico in una certa direzione
    void LookAtDirection(Vector3 direction)
    {
        if (!isAttacking)
        {
            // Calcoliamo la rotazione necessaria per far guardare l'entità nella direzione specificata
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // Aggiorniamo la rotazione dell'entità per farla girare gradualmente verso la rotazione target
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Metodo per iniziare l'inseguimento
    void StartChase()
    {
        Debug.Log("Inizio inseguimento.");
        agent.isStopped = false;  // Facciamo ripartire l'agente
        agent.SetDestination(player.transform.position);  // Impostiamo la destinazione dell'agente sulla posizione del giocatore
        animator.SetBool("grounded", true);  // Impostiamo l'animazione a terra
        animator.SetBool("attack", false);  // Impostiamo l'animazione di attacco a false
        animator.SetFloat("Velocity", agent.velocity.magnitude);  // Impostiamo la velocità dell'animazione sulla velocità dell'agente
        agent.updateRotation = false;  // Impediamo all'agente di aggiornare automaticamente la sua rotazione
    }

    // Metodo per iniziare lo stato idle
    void StartIdle()
    {
        Debug.Log("Inizio stato idle.");
        agent.isStopped = true;  // Fermiamo l'agente
        animator.SetBool("grounded", true);  // Impostiamo l'animazione a terra
        animator.SetBool("attack", false);  // Impostiamo l'animazione di attacco a false
        animator.SetFloat("Velocity", 0f);  // Impostiamo la velocità dell'animazione a 0
        // Fissiamo la rotazione Y del nemico per evitare rotazioni indesiderate
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        agent.updateRotation = false;  // Impediamo all'agente di aggiornare automaticamente la sua rotazione
    }

    // Metodo per far guardare il nemico al giocatore
    void LookAtPlayer()
    {
        // Calcoliamo la direzione dal nemico al giocatore
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // Calcoliamo la rotazione necessaria per far guardare l'entità nella direzione specificata
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // Aggiorniamo la rotazione dell'entità per farla girare gradualmente verso la rotazione target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Coroutine per eseguire l'attacco
    // Coroutine per eseguire l'attacco
    // Coroutine per eseguire l'attacco
    IEnumerator PerformAttack()
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

}
