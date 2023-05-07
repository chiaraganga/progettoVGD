using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private CharacterController ch;

    Vector3 movement;

    //Parametri di movimento
    float Horizontal_mov;
    float Vertical_mov;
    public float speed;
    public float rotationSpeed = 100f;
    public float jumpForce = 10;
    private const float gravity = 9.81f;
    public float vspeed;
    public float Knock_Back_Force = 1f;
    public float Knock_Back_Time;
    private float Knock_Back_Max_Time = 1f;
    
    

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ch = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Knock_Back_Time <= 0)
        {
            
            Horizontal_mov = Input.GetAxis("Horizontal")*rotationSpeed * Time.deltaTime; //prendiamo da tastiera i movimenti per gli assi x e z
            Vertical_mov = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            movement = (ch.transform.forward * Vertical_mov);
            




            if (ch.isGrounded)// se il character controller � ancorato a terra allora posso saltare
            {

                vspeed = 0;

                float jump = Input.GetAxis("Jump") * jumpForce;
                if (jump > 0)
                    vspeed = jump;
            }
            else
                vspeed -= gravity * Time.deltaTime;
        }





        else
        {
            Knock_Back_Time -= Time.deltaTime;
        }
        // simuliamo la forza di gravit� in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno
        
        movement.y = vspeed;
        if (vspeed < 0)
            vspeed = 0;
        //movimento e rotazione

        ch.transform.Rotate(Vector3.up * Horizontal_mov );
        ch.Move(movement);
        









    }
    public void KnockBack(Vector3 distance)
    {
        //durata e direzione del nostro contraccolpo 
        Knock_Back_Time = Knock_Back_Max_Time;
        ch.Move(distance * Knock_Back_Force);
        movement.y = Knock_Back_Force;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collect"))
        {
            other.gameObject.SetActive(false);

        }
    }
}