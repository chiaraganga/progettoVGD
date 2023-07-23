using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

[System.Serializable]
public class GameData
{
    public SerializableVector3 position;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

public class Player_controller : MonoBehaviour
{
    private CharacterController ch;
    private GameObject Zeus;
    private GameObject spada;
    private Vector3 movement;
    private int buildIndex;
    private float Horizontal_mov;
    private float Vertical_mov;
    public float rotationSpeed;
    private const float gravity = 0.981f;
    public float vspeed = 0;
    private float jump;
    public float jump_force;
    private int score = 0;
    private string saveDataPath;
    private bool is_jumping = false;
    private bool double_jump = false;
    private bool run = false;
    private Animator animator;
    private float velocity = 0;
    private float lateralVelocity = 0;

    public float walkSpeed = 0.1f;
    public float runSpeed = 1.0f; // Imposta la velocità di corsa desiderata

    public float horizontalRotationSpeed = 2.0f;
    public float verticalRotationSpeed = 2.0f;

    public Health_manager healthManager; // Riferimento al componente Health_manager
    public GameObject player; // Riferimento al GameObject del giocatore
    private NavMeshAgent agent; // Riferimento al componente NavMeshAgent

    private void Start()
    {
        // Recupera l'indice della scena attuale
        Scene currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;

        // Disattiva Zeus e la spada se l'indice della scena è 0, 2 o 3
        if (buildIndex == 0)
        {
            Zeus = GameObject.FindGameObjectWithTag("Zeus");
            Zeus.SetActive(false);
            spada = GameObject.FindGameObjectWithTag("Weapon");
            spada.SetActive(false);
        }
        if (buildIndex == 2 || buildIndex == 3)
        {
            spada = GameObject.FindGameObjectWithTag("Weapon");
            spada.SetActive(false);
        }

        // Blocca il cursore nel centro dello schermo
        Cursor.lockState = CursorLockMode.Locked;

        // Ottieni il riferimento al CharacterController e all'Animator
        ch = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Trova l'oggetto del giocatore precedente e distruggilo
        GameObject oldplayer = GameObject.Find("Player");
        if (oldplayer != this.gameObject)
        {
            Destroy(oldplayer);
        }
    }

    private void Update()
{
    if (player != null && !healthManager.death)
    {
        if (PauseMenu.IsPaused())
        {
            return;
        }

        animator.SetBool("attack", false);

        if (ch.enabled == true)
        {
            Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            Vertical_mov = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

            if (Mathf.Abs(Horizontal_mov) > 0)
            {
                lateralVelocity = Mathf.Abs(Horizontal_mov);
            }
            else
            {
                lateralVelocity = 0;
            }

            animator.SetBool("dialogues", false);
        }
        else
        {
            animator.SetBool("dialogues", true);
        }

        run = Input.GetKey(KeyCode.CapsLock) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button4);
        float playerSpeed = run ? runSpeed : walkSpeed;

        if (ch.isGrounded)
        {
            if (vspeed < 0f)
                vspeed = -0.2f;

            if (is_jumping == true)
            {
                is_jumping = false;
                animator.SetBool("grounded", false);
            }
        }

        if (Input.GetButtonDown("Jump") && ch.isGrounded)
        {
            is_jumping = true;
            vspeed = jump_force;
            animator.SetBool("grounded", true);
            double_jump = true;
        }
        else
        {
            if (Input.GetButtonDown("Jump") && double_jump == true)
            {
                double_jump = false;
                vspeed = jump_force;
                playerSpeed = 0.08f; // Imposta la velocità del giocatore durante il doppio salto
            }
            else
            {
                playerSpeed = walkSpeed; // Imposta la velocità di camminata solo se non sta correndo né facendo un doppio salto
            }
        }

        if (Vertical_mov > 0)
        {
            velocity = run ? 1.5f : 0.5f; // Imposta la velocità del giocatore in base a se sta correndo o camminando
        }
        else if (Vertical_mov < 0)
        {
            velocity = -0.5f;
        }
        else
        {
            velocity = 0f;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            animator.SetBool("attack", true);
        }

        animator.SetFloat("Velocity", velocity);
        animator.SetFloat("LateralVelocity", lateralVelocity);

        Vector3 forwardMovement = transform.forward * Vertical_mov;
        Vector3 rightMovement = transform.right * Horizontal_mov;
        movement = (forwardMovement + rightMovement) * playerSpeed;
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;

        if (ch.enabled == true)
        {
            ch.Move(movement);

            float rightStickHorizontal = Input.GetAxis("RightStickHorizontal");
            float rightStickVertical = Input.GetAxis("RightStickVertical");

            if (Mathf.Abs(rightStickHorizontal) > 0.1f || Mathf.Abs(rightStickVertical) > 0.1f)
            {
                transform.Rotate(new Vector3(rightStickVertical * verticalRotationSpeed, rightStickHorizontal * horizontalRotationSpeed, 0));
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            save();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            load();
        }
    }
    else if (player != null && healthManager.death)
    {
        animator.SetBool("grounded", true);
        animator.SetBool("attack", false);
        animator.SetFloat("Velocity", agent.velocity.magnitude);
    }

    if (Input.GetKey(KeyCode.Q))
    {
        transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
    }
    // Ruota il giocatore a destra quando si preme E
    if (Input.GetKey(KeyCode.E))
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collect"))
        {
            other.gameObject.SetActive(false);
            score++;
        }

        if (score == 1 && buildIndex == 0)
        {
            Zeus.SetActive(true);
        }

        if (other.CompareTag("Weapon"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Next_Level"))
        {
            PlayerPrefs.SetInt("currentlevel", (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            if (PlayerPrefs.GetInt("currentlevel") == 6)
                Destroy(this.gameObject);
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentlevel"));
        }
    }

    public void save()
    {
        saveDataPath = Application.persistentDataPath + "/data.vgd";
        GameData gameData = new GameData();
        gameData.position = new SerializableVector3(transform.position);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.Create);

        formatter.Serialize(fileStream, gameData);
        fileStream.Close();
    }

    public void load()
    {
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveDataPath, FileMode.Open);

            GameData gameData = (GameData)formatter.Deserialize(fileStream);

            transform.position = gameData.position.ToVector3();
            fileStream.Close();
        }
    }
}
