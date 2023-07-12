using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health_manager : MonoBehaviour
{
    private Animator anim; // Riferimento all'Animator del personaggio
    public health_bar barra_vita; // Riferimento alla barra della vita (classe health_bar)
    public int health; // Vita attuale del personaggio
    public int max_health = 15; // Vita massima del personaggio
    public bool death = false; // Flag per indicare se il personaggio è morto
    private GameObject Ares; // Riferimento all'oggetto Ares (se esiste)

    private bool isDying = false; // Flag per tenere traccia dello stato di animazione di morte

    public static int enemiesDead = 0; // Contatore per nemici morti. Questo dovrebbe essere statico perché deve essere condiviso tra tutti gli oggetti Health_manager.

    public GameObject winObject; // Riferimento al GameObject "win".


    public void Start()
    {
        anim = GetComponent<Animator>(); // Ottenere il riferimento all'Animator del personaggio
        Ares = GameObject.FindGameObjectWithTag("Ares"); // Ottenere il riferimento all'oggetto Ares tramite il tag
        health = max_health; // Impostare la vita iniziale a quella massima
        barra_vita.Set_max_health(max_health); // Impostare la vita massima nella barra della vita

        // Trova il GameObject "win" nel tuo gioco. Assicurati che esista un oggetto con questo nome.
        winObject = GameObject.Find("win");
    }

        public void Update()
    {
        if (health <= 0 && !isDying && death==false ) //se la salute è minore di zero
        {
            Debug.Log(gameObject.name + " sta morendo con una salute di: " + health);
            death = true; // Il personaggio è morto
            anim.SetBool("death", true); // Impostare il parametro "death" nell'Animator per avviare l'animazione di morte
            isDying = true; // Il personaggio sta entrando nella fase di animazione di morte
            Invoke("CompleteDeathAnimation", 4.4f); // Aggiungere un ritardo per completare l'animazione di morte
        }
    }

            public void Damages(int damage)
    {
        Debug.Log(gameObject.name + " ha subito " + damage + " danni.");
        health -= damage; // Subtract damage from character's life

        if (health < 0)
        {
            health = 0; // Ensure health never goes below 0
        }

        Debug.Log(gameObject.name + " La salute rimanente è " + health + ".");
        barra_vita.Set_health(health); // Update health bar
    }

    public void Healing(int heal)
    {
        health += heal; // Aggiungere la quantità di cura alla vita del personaggio
        if (health > max_health)
        {
            health = max_health; // Assicurarsi che la vita non superi la vita massima
        }
    }

    public void destroy_anim()
    {
        barra_vita.Set_max_health(0); // Impostare la vita massima a zero nella barra della vita
        SceneManager.LoadScene(6); // Caricare la scena 6 (potrebbe essere necessario modificare il numero della scena in base al tuo progetto)
    }

    public void CompleteDeathAnimation()
    {
        if (Ares != null)
        {
            Ares.SetActive(true); // Attivare l'oggetto Ares (se esiste)
            enemiesDead += 1;
        }
        
        Destroy(gameObject); // Distruggere il personaggio
        // Aumenta il contatore dei nemici morti.
        enemiesDead += 1;

        // Se entrambi i nemici sono morti...
        if (enemiesDead >= 3)
        {
            GameObject parentObject = GameObject.Find("ParentObject");
                if (parentObject != null)
                {
                    winObject = parentObject.transform.Find("win").gameObject;
                    if(winObject != null) 
                    {
                        //Attiva il GameObject "win"
                        winObject.SetActive(true);
                        Debug.Log("Il GameObject 'win' è stato attivato");
                    }
                    else 
                    {
                        Debug.LogError("'win' GameObject non trovato come figlio di ParentObject");
                    }
                }
                else
                {
                    Debug.LogError("ParentObject non trovato");
                }

                                    
        }
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Player") && other.CompareTag("Collect"))
        {
            Healing(10); // Applicare una cura al personaggio quando entra in collisione con un oggetto di raccolta
        }
    }

    
}
