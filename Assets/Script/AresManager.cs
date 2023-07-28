using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresManager : MonoBehaviour
{
    public GameObject Guerriero;
    public GameObject Ares;

    void Update()
    {
        // Verifica se il Guerriero è sconfitto
        if(Guerriero == null) 
        {
            // Attiva Ares se il Guerriero è sconfitto
            Health_manager.enemiesDead = 2 ;
            Ares.SetActive(true);
        }
    }
}
