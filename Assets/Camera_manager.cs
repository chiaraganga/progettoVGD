using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Vector3 offset;
    public bool offset_values;
    public float rotation_speed;

    // Start is called before the first frame update
    void Start()
    {
        if (!offset_values)
        offset = player.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        
        transform.position =(player.position - offset);
       
        
    }
   
}
