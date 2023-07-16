using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{


    public void Livello0()
    {
        SceneManager.LoadScene("Livello 0");
    }
    public void Livello1()
    {
        SceneManager.LoadScene("Livello 1");
    }
    public void Livello2()
    {
        SceneManager.LoadScene("Livello 2");
    }
    public void Livello3()
    {
        SceneManager.LoadScene("Livello 3");
    }
    public void Livello4()
    {
        SceneManager.LoadScene("Livello 4");
    }

    public void Crediti()
    {
        SceneManager.LoadScene("Credits");
    }

    public void StrtMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


    /* Funzione per il caricamento della scena salvata */
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("currentlevel") > 0)
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentlevel"));
    }


    /* Funzione per il salvataggio della scena */
    public void SaveScene() {
        PlayerPrefs.SetInt("currentlevel", SceneManager.GetActiveScene().buildIndex);

    }

}
