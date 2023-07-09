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
    public bool dead = false; // Flag per indicare se il personaggio è morto
    private GameObject Ares; // Riferimento all'oggetto Ares (se esiste)

    private bool isDying = false; // Flag per tenere traccia dello stato di animazione di morte

    public void Start()
    {
        anim = GetComponent<Animator>(); // Ottenere il riferimento all'Animator del personaggio
        Ares = GameObject.FindGameObjectWithTag("Ares"); // Ottenere il riferimento all'oggetto Ares tramite il tag
        health = max_health; // Impostare la vita iniziale a quella massima
        barra_vita.Set_max_health(max_health); // Impostare la vita massima nella barra della vita
    }

    public void Update()
    {
        if (health <= 0 && !dead && !isDying)
        {
            dead = true; // Il personaggio è morto
            anim.SetBool("dead", true); // Impostare il parametro "dead" nell'Animator per avviare l'animazione di morte
            isDying = true; // Il personaggio sta entrando nella fase di animazione di morte
            Invoke("CompleteDeathAnimation", 4.4f); // Aggiungere un ritardo per completare l'animazione di morte
        }
    }

    public void Damages(int damage)
    {
        health -= damage; // Sottrarre il danno dalla vita del personaggio
        barra_vita.Set_health(health); // Aggiornare la barra della vita
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
        }

        Destroy(gameObject); // Distruggere il personaggio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Player") && other.CompareTag("Collect"))
        {
            Healing(10); // Applicare una cura al personaggio quando entra in collisione con un oggetto di raccolta
        }
    }
}
