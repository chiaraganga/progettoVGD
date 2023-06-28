using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    private string currentScene;

    private void Awake()
    {
        // Memorizza il nome della scena corrente all'avvio del gioco
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void OnRetryButtonClicked()
    {
        // Carica la scena corrente
        SceneManager.LoadScene(currentScene);
    }
}
