using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staute : MonoBehaviour
{
    public List<GameObject> statuette;
    private GameObject Zeus;
    public int Score = 0;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        Zeus = GameObject.FindGameObjectWithTag("Zeus");
        Zeus.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        for (i = 0; i < statuette.Count; i++)
        {
            if (statuette[i].active == false)
                Destroy(statuette[i]);
                Score++;





        }



        if (Score == 2)
            Zeus.SetActive(true);
    }
}
   
        






