using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToLv2 : MonoBehaviour
{
    public string playerTag = "Player";
    public string nextSceneName = "Livello 2";


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
            Health_manager.enemiesDead = 0 ;
            
            SceneManager.LoadScene("Livello 2");
        }
    }
}
