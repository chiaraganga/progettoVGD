using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private string previousSceneName;

    void Start() {
        Cursor.lockState = CursorLockMode.None; // Sblocca il cursore del mouse
        Cursor.visible = true; // Rendi il cursore del mouse visibile
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Iscriviti all'evento sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Questo metodo viene chiamato ogni volta che una scena viene caricata
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GameOver") // Se la scena non è GameOver, allora è la scena di gioco
        {
            previousSceneName = scene.name;
        }
    }

    public void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void GoBackToPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogError("Cannot load previous scene because the scene name is not set.");
        }
    }

    public void OnBackToGameButtonPressed()
    {
        GoBackToPreviousScene();
    }

    private void OnDestroy()
    {
        // Annulla l'iscrizione all'evento sceneLoaded quando questo oggetto viene distrutto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
