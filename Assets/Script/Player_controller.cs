using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public SerializableVector3 position;
}

[System.Serializable]
public struct SerializableVector3  //creo una struttura di tipo (in questo caso)SerializableVector3
{
    float x, y, z;  // float di appoggio con i nomi degli assi

    public SerializableVector3(Vector3 v) //richiamo la struttura passandogli come parametro un vector3 
    {
        x = v.x; //metto  nelle variabili della struttura i valori di ogni asse del vector3
        y = v.y;
        z = v.z;
    }

    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}


public class Player_controller : MonoBehaviour
{
    
    private CharacterController ch;
    private GameObject Zeus;
    private GameObject spada;
    Vector3 movement;
    int buildIndex;
    //Parametri di movimento
    float Horizontal_mov;
    float Vertical_mov;
    public float rotationSpeed;
    private const float gravity = 0.981f;
    public float vspeed = 0;
    
    private float jump;
    public float jump_force;
    private  int score = 0;
    string saveDataPath;
    private bool is_jumping=false;
    private bool double_jump = false;
    private bool run = false;
    private float coeff_vel;
    
    private Animator animator;
    float velocity = 0;
    float lateralVelocity = 0;

    private void Awake()
    {

       
       
    }
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;
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

        Cursor.lockState = CursorLockMode.Locked;
        ch = GetComponent<CharacterController>();

        
        animator = GetComponent<Animator>();
        
        GameObject oldplayer = GameObject.Find("Player");
        if (oldplayer != this.gameObject)
        {
            Destroy(oldplayer);
        }
        

    }


    // Update is called once per frame
    void Update()
    {   
        if(PauseMenu.IsPaused()){
            return;
        }
        animator.SetBool("attack", false);
        if(ch.enabled == true) {
            Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            Vertical_mov = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

            // Assegna la magnitudine di Horizontal_mov a lateralVelocity
            lateralVelocity = Mathf.Abs(Horizontal_mov);

            animator.SetBool("dialogues", false);
        } else {
            animator.SetBool("dialogues", true);
        }
       
       
        if (Input.GetKey(KeyCode.CapsLock) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button4))
        {
            run = true;
            
        }
        else
        {

            run = false;
        }

        if (ch.isGrounded)// se il character controller è ancorato a terra allora posso saltare
        {
            if (vspeed < 0f)
                vspeed = -0.2f;

            if(is_jumping==true)
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
            if(Input.GetButtonDown("Jump") && double_jump==true)
            {

                double_jump = false;
                vspeed =jump_force;
                coeff_vel = 0.08f;
            }
            
            coeff_vel = 0.1f;
        }


        if (Vertical_mov > 0)
        {
            if (run == true)
            {
                velocity = 1.5f;
            }
            else
            {
                velocity = 0.5f;
            }
        }
        if (Vertical_mov == 0)
            velocity = 0f;
        if (Vertical_mov < 0)
            velocity = -0.5f;

        if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            animator.SetBool("attack", true);
        }
       
        animator.SetFloat("Velocity", velocity);
        animator.SetFloat("LateralVelocity", lateralVelocity); //assegna a LateralVelocity nella tua animazione il valore della variabile lateralVelocity.


        Vector3 forwardMovement = transform.forward * Vertical_mov;
        Vector3 rightMovement = transform.right * Horizontal_mov;

        movement = (forwardMovement + rightMovement) * coeff_vel;
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;
        if(ch.enabled==true)
        {
            ch.Move(movement);
        }

        if (Input.GetKeyDown("p"))
        {
            save();
        }
        if (Input.GetKeyDown("o"))
        {
            load();
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
        saveDataPath = Application.persistentDataPath + "/data.vgd"; //salva in una cartella che è uguale per tutti gli os usando una estensione che non esoste
        GameData gameData = new GameData();
        gameData.position = new SerializableVector3(transform.position);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.Create);

        formatter.Serialize(fileStream, gameData);
    }
    public void load()
    {
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveDataPath, FileMode.Open);

            GameData gameData = (GameData)formatter.Deserialize(fileStream);

            transform.position = gameData.position.toVector3();
        }
    }
    
}
