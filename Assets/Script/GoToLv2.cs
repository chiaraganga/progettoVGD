using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToLv2 : MonoBehaviour
{
    public string playerTag = "Player"; // Assegna il tag del tuo giocatore qui. "Player".
    public string nextSceneName = "Livello 2"; // Assegna il nome della scena a cui vuoi passare qui.


    // Start is called before the first frame update
    void Start()
    {
        // Disabilita questo GameObject all'avvio
        gameObject.SetActive(false);
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Se l'oggetto che è entrato nel trigger ha il tag del giocatore...
        if (other.CompareTag(playerTag))
        {
            Health_manager.enemiesDead = 0 ;
            // Carica la scena dei crediti
            SceneManager.LoadScene("Livello 2");
        }
    }
}
