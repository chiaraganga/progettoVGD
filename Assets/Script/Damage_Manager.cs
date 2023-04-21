using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Manager : MonoBehaviour
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

        if (other.CompareTag("player"))
        {
            Vector3 hit_direction = other.transform.position - transform.position;// creo la direzione in cui il personaggio dovrà andare quando subirà un danno
            hit_direction = hit_direction.normalized;// per evitare distanze troppo grandi normalizziamo il vettore che quindi avrà stessa direzione ma distaza unitaria
            FindObjectOfType<Health_Manager>().Damages(damage_done, hit_direction);//cerca tutti gli oggetti che hanno attaccato come script healthmanager(in questo caso c'è solo il nostro player)
                                                                                  //richiama il metodo damages e gli passa come parametro il danno che vogliamo venga inflitto al personaggio e la direzione in cui deve essere respinto
                                                                                  // in alternativa possiamo anche utilizzare questa sintassi other.gameObject.GetComponent<HealthManager>().Damages(damage_done,hit_direction)


        }
    }
}