using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_rotation : MonoBehaviour
{
    public Vector2 turn_movement;
    public float rotationSpeed = 10f;
    
    float speed = 1;
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
    }
       
       
}
