using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public string playerTag = "Player"; // tag del giocatore.
    public string nextSceneName = "GameWin"; // nome della scena dove passare qui.

    // Start is called before the first frame update
    void Start()
    {
        // Disabilita questo GameObject all'avvio
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Se l'oggetto che è entrato nel trigger ha il tag del giocatore
        if (other.CompareTag(playerTag))
        {
            // Carica la scena della vittoria
            SceneManager.LoadScene("GameWin");
        }
    }
}

