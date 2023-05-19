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
    private const float gravity = 9.81f;
    public float vspeed = 0;
    public float Knock_Back_Force = 1.0f;
    public float Knock_Back_Time;
    private const float Knock_Back_Max_Time = 1.0f;
    public float jump_force;
    public int score = 0;
    string saveDataPath;
    private bool running;
    private bool grounded;
    public float walk_speed;
    public float run_speed;
    private float real_speed;
    public int coeff_vel;
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
        real_speed = walk_speed;


        Cursor.lockState = CursorLockMode.Locked;
        ch = GetComponent<CharacterController>();

        //Aggiunto da Chiara per le animazioni 
        animator = GetComponent<Animator>();
        //
        GameObject oldplayer = GameObject.Find("Player");
        if (oldplayer != this.gameObject)
        {
            Destroy(oldplayer);
        }

    }


    // Update is called once per frame
    void Update()
    {

        animator.SetBool("grounded", ch.isGrounded);

        float Horizontal_mov = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime; //prendiamo da tastiera i movimenti per gli assi x e z
        float Vertical_mov = Input.GetAxis("Vertical") * Time.deltaTime;









        if (ch.isGrounded)// se il character controller   ancorato a terra allora posso saltare
        {


            if (vspeed < 0f)
                vspeed = -2f;




            if (Input.GetButtonDown("Jump"))
            {

                float jump = Input.GetAxis("Jump");
                vspeed = jump * jump_force;


            }


        }


        //messa parte per  le animazioni da controllare
        if (Vertical_mov > 0)
            velocity += Time.deltaTime * 0.2f;
        else
            velocity -= Time.deltaTime * 0.8f;



        velocity = Mathf.Clamp01(velocity); //clamp01 restituisce un valore compreso tra 0 e 1, quindi se velocity diventa maggiore di 1 la clamp ci riporta al valore 1
        // se velocity diventa minore di 0 la clamp ci riporta a 0, in modo da non ottenere velocità indesiderate
        animator.SetFloat("Velocity", velocity);//nell'animator assegna a velocity(quella delle animazioni)  la nostra variabile da input

        movement = ch.transform.forward * velocity * coeff_vel;

        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;

        ch.Move(movement);
        //tranform.forward è un vettore unitario relativo all'oggetto, sarà sempre nella direzione in cui punta il "davanti" dell'oggetto

        transform.Rotate(Vector3.up, Horizontal_mov * 0.2f);






        // simuliamo la forza di gravità  in modo che quando siamo in aria il nostro personaggio torni attaccato al terreno
        vspeed -= gravity * Time.deltaTime;
        movement.y = vspeed;
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
