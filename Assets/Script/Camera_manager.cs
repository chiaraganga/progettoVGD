using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    
    public bool offset_values;
    public float plerp=0.05f;
    public float rlerp=0.05f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, plerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, rlerp);

    }

    // Update is called once per frame
    void Update()
    {
        





    }

}
