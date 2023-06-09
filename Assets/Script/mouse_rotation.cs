using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_rotation : MonoBehaviour
{
    private GameObject player;
    private CharacterController ch;
    public Vector2 turn_movement;
    public Vector3 movement;
    public float rotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        turn_movement.y += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        turn_movement.x += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
       

        transform.localRotation = Quaternion.Euler(-turn_movement.y, turn_movement.x, 0f);
        if(Input.GetButtonDown("Vertical"))
        {
            
            player = GameObject.FindGameObjectWithTag("Player");
            
            if (player.transform.position!=transform.position)
            {
               
                player.transform.Rotate(Vector3.up, turn_movement.x);
                
                turn_movement.y = 0f;
                turn_movement.x = 0f;
                
                transform.localRotation = Quaternion.Euler(-turn_movement.y, turn_movement.x, 0f);
                
            }
            

        }
    }
       
       
}
