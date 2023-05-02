using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private CharacterController ch;

    Vector3 movement;
    //Parametri di movimento
    public float speed = 5;
    public float rotationSpeed = 720f;
    public float jumpSpeed = 10;
    private const float gravity = 9.81f;
    private float vspeed = 0;
    public float Knock_Back_Force = 1f;
    public float Knock_Back_Time;
    private float Knock_Back_Max_Time = 1f;

    void Start()
    {
        ch = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Knock_Back_Time <= 0)
        {
            float Horizontal_mov = Input.GetAxis("Horizontal"); //prendiamo da tastiera i movimenti per gli assi x e z
            float Vertical_mov = Input.GetAxis("Vertical");
            movement = new Vector3(Horizontal_mov, 0f, Vertical_mov) * speed;
            if (ch.isGrounded)// se il character controller � ancorato a terra allora posso saltare
            {
                float jump = Input.GetAxis("Jump") * jumpSpeed;
                vspeed = jump;


            }
        }





        else
        {
            Knock_Back_Time -= Time.deltaTime;
        }
        // simuliamo la forza di gravit� in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;
        ch.Move(movement * Time.deltaTime);
        //Rotazione del giocatore verso dove sta guardando
        if (movement.x != 0 || movement.z != 0)
        {
            Quaternion Player_Rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Player_Rotation, rotationSpeed * Time.deltaTime);

        }

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