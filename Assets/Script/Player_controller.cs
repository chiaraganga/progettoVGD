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
    Vector3 movement;
    int buildIndex;
    //Parametri di movimento
    float Horizontal_mov;
    float Vertical_mov;
    public float rotationSpeed;
    private const float gravity = 0.981f;
    public float vspeed = 0;
    public float Knock_Back_Force = 1.0f;
    public float Knock_Back_Time;
    private float jump;
    public float jump_force;
    public int score = 0;
    string saveDataPath;
    private bool is_jumping=false;
    private bool double_jump = false;
    private bool run = false;
    private float coeff_vel;
    
    private Animator animator;
    float velocity = 0;

    private void Awake()
    {

        
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        buildIndex = currentScene.buildIndex;
        if (buildIndex == 0)
        {
            Zeus = GameObject.FindGameObjectWithTag("Zeus");
            Zeus.SetActive(false);
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


        animator.SetBool("attack", false);
        Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; //prendiamo da tastiera i movimenti per gli assi x e z
        Vertical_mov = Input.GetAxis("Vertical") * Time.deltaTime;
        jump = Input.GetAxis("Jump");
        if (Input.GetKey(KeyCode.CapsLock) || Input.GetKey(KeyCode.Joystick1Button4))
        {
            run = true;
            
        }
        else
        {

            run = false;
        }

        if (ch.isGrounded)// se il character controller   ancorato a terra allora posso saltare
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
            vspeed = jump * jump_force;
            animator.SetBool("grounded", true);
            coeff_vel = 0.08f;
            double_jump = true;
            


        }
        else
        {
            if(Input.GetButtonDown("Jump") && double_jump==true)
            {

                double_jump = false;
                vspeed = jump * jump_force;
                coeff_vel = 0.05f;
            }
            vspeed -= gravity * Time.deltaTime;
            coeff_vel = 0.1f;
        }


        if (Vertical_mov > 0)
        {
            if (run == true)
            {
                velocity = 2f;
            }
            else
            {
                velocity = 1f;
            }
        }
        if (Vertical_mov == 0)
            velocity = 0f;
        if (Vertical_mov < 0)
            velocity = -0.3f;




        if(Input.GetMouseButtonDown(0) )
        {
            animator.SetBool("attack", true);
        }
       
    

        animator.SetFloat("Velocity", velocity);//nell'animator assegna a velocity(quella delle animazioni)  la nostra variabile da input
        
        movement = ch.transform.forward * velocity * coeff_vel;//tranform.forward è un vettore unitario relativo all'oggetto, sarà sempre nella direzione in cui punta il "davanti" dell'oggetto

        
        movement.y = vspeed;

        ch.Move(movement);


        transform.Rotate(Vector3.up, Horizontal_mov * 0.2f);






        // simuliamo la forza di gravità  in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno

        //movimento e rotazione




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
        if (score == 2 && buildIndex == 0)



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
