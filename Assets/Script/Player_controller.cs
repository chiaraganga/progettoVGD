using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    private CharacterController ch;
    public GameObject gos;
    Vector3 movement;

    //Parametri di movimento
    float Horizontal_mov;
    float Vertical_mov;
    public float speed;
    public float rotationSpeed;
    private const float gravity = 9.81f;
    public float vspeed = 0;
    public float Knock_Back_Force = 1.0f;
    public float Knock_Back_Time;
    private const float Knock_Back_Max_Time = 1.0f;
    public float jump_force;
    public int score = 0;

    void Start()
    {


        Cursor.lockState = CursorLockMode.Locked;
        ch = GetComponent<CharacterController>();
        gos.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; //prendiamo da tastiera i movimenti per gli assi x e z
        Vertical_mov = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        movement = (ch.transform.forward * Vertical_mov);





        if (ch.isGrounded)// se il character controller � ancorato a terra allora posso saltare
        {

            if (vspeed < 0)
                vspeed = -2f;

            float jump = Input.GetAxis("Jump");
            if (Input.GetButtonDown("Jump"))
            {


                vspeed = jump * jump_force;
            }
        }


        // simuliamo la forza di gravit� in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;
        //movimento e rotazione

        ch.transform.Rotate(Vector3.up * Horizontal_mov);
        ch.Move(movement);
        if (score == 5)
        {
            gos.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Collect"))
        {

            other.gameObject.SetActive(false);
            score += 1;
        }
    }
}   
    