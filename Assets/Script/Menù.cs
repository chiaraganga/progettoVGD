using UnityEngine;
using UnityEngine.SceneManagement;

public class Menù : MonoBehaviour
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

    public void StrtMenù()
    {
        SceneManager.LoadScene("Menù");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void NewGame()
    {

        
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("currentlevel", 0);
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentlevel"));

    }
    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentlevel"));
       
    }
}
