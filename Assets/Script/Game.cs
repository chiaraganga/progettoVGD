using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player; // Assegna il tuo player qui dall'inspector
    public GameObject gameOverMenu; // Assegna il tuo GameOver Menu qui dall'inspector
    public PauseMenu pauseMenu; // Riferimento allo script del menu

    void Update()
    {
        // Controlla se il player non esiste più
        if (player == null)
        {
            // Se il player non esiste più, attiva il menù GameOver
            gameOverMenu.SetActive(true);
            pauseMenu.DisableMenu();

            // Riattiva il cursore del mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        
    }
}
