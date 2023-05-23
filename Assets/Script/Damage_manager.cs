using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_manager : MonoBehaviour
{
    public int damage_done = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

            FindObjectOfType<Health_manager>().Damages(damage_done);//cerca tutti gli oggetti che hanno attaccato come script healthmanager
                                                                                   //richiama il metodo damages e gli passa come parametro il danno che vogliamo venga inflitto al personaggio e la direzione in cui deve essere respinto
                                                                                   // in alternativa possiamo anche utilizzare questa sintassi other.gameObject.GetComponent<HealthManager>().Damages(damage_done,hit_direction)


        
    }
}