using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CountDown_manager timer;
    public IEnumerator Corutine_timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<CountDown_manager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("checkpoint"))
        {
            if (other.CompareTag("Player"))
            {
                timer.time_to_end += 5;
            }
        }
        if (this.CompareTag("Finish"))
        {
            if (other.CompareTag("Player"))
            {
                timer.finish = true;
            }
            
        }

    }
}
