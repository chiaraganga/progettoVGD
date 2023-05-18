
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
    public float speed;
    public float rotationSpeed;
    private const float gravity = 9.81f;
    public float vspeed = 0;
    public float Knock_Back_Force = 1.0f;
    public float Knock_Back_Time;
    private const float Knock_Back_Max_Time = 1.0f;
    public float jump_force;
    public int score = 0;
    string saveDataPath;

    //Aggiunto da Chiara per le animazioni
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

        //Aggiunto da Chiara per le animazioni 
        animator = GetComponent<Animator>();

        GameObject oldplayer = GameObject.Find("Player");
        if (oldplayer != this.gameObject)
        {
            Destroy(oldplayer);
        }

    }

     //Aggiunto float per le animazioni
    // Update is called once per frame
    void Update()
    {
        float Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; //prendiamo da tastiera i movimenti per gli assi x e z
        float Vertical_mov = Input.GetAxis("Vertical") * speed * Time.deltaTime;
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

        //messa parte per le animazioni da controllare
        if (Vertical_mov > 0)
        {
            velocity += Time.deltaTime * 0.2f;
        } else
        {
            velocity -= Time.deltaTime * 0.5f;
        }

        velocity = Mathf.Clamp01(velocity);
        animator.SetFloat("Velocity", velocity);
        animator.SetFloat("Turn", Horizontal_mov);

        ch.SimpleMove(transform.forward * velocity);


        // simuliamo la forza di gravit� in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;
        //movimento e rotazione

        ch.transform.Rotate(Vector3.up * Horizontal_mov * Time.deltaTime* 0.2f);
        ch.Move(movement);

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
        saveDataPath = Application.persistentDataPath + "/data.vdg"; //salva in una cartella che è uguale per tutti gli os usando una estensione che non esoste
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
    