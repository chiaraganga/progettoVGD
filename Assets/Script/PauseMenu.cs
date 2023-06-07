using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject menuPrincipaleUI;
    public GameObject menuLivelliUI;

    private bool isPaused = false;
    private bool isMainMenuActive = true;

    private void Start()
    {
        ResumeGame(); // Inizia il gioco con il menu di pausa chiuso
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Congela il tempo di gioco per fermare la logica del gioco
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Sblocca il cursore del mouse
        Cursor.visible = true; // Rendi il cursore del mouse visibile
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Ripristina il tempo di gioco normale
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Blocca il cursore del mouse
        Cursor.visible = false; // Rendi il cursore del mouse invisibile
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoToMenuPrincipale()
    {
        if (!isMainMenuActive)
        {
            menuPrincipaleUI.SetActive(true);
            menuLivelliUI.SetActive(false);
            isMainMenuActive = true;
        }
    }

    public void GoToMenuLivelli()
    {
        if (isMainMenuActive)
        {
            menuPrincipaleUI.SetActive(false);
            menuLivelliUI.SetActive(true);
            isMainMenuActive = false;
        }
    }
}