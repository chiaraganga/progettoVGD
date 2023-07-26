using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player; // Assegna il tuo player qui dall'inspector
    public GameObject gameOverMenu; // Assegna il tuo GameOver Menu qui dall'inspector
    public PauseMenu pauseMenu; // Riferimento allo script del menu
    public GameObject dialogPanel;
    
    public static bool isGameOver = false; // Nuova variabile statica

    private void OnEnable() 
    {
        isGameOver = false; // Quando il gioco inizia o quando una scena viene ricaricata, isGameOver viene resettato a false
    }

    void Update()
    {
        // Controlla se il player non esiste più
        if (player == null)
        {
            // Se il player non esiste più, attiva il menù GameOver
            gameOverMenu.SetActive(true);
            pauseMenu.DisableMenu();
            dialogPanel.SetActive(false);

            // Riattiva il cursore del mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            isGameOver = true; // Imposta isGameOver a true
        }
    }
}
