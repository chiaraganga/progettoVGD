using UnityEngine;

public class ActiveCredits : MonoBehaviour
{
    public Animator creditsAnimator; // Riferimento all'Animator del Canvas "crediti".
    public GameObject creditsCanvas;

    // Assicurati di impostare il riferimento nel pannello dell'inspector di Unity, o puoi anche cercare il figlio nel codice se preferisci.

    private void Start()
    {
        // Nasconde i crediti all'avvio.
        creditsCanvas.SetActive(false);
        creditsAnimator.gameObject.SetActive(false);

        // Assicurati che questo oggetto non venga distrutto quando si carica una nuova scena.
        //DontDestroyOnLoad(this.gameObject);
    }

    // Funzione per mostrare i crediti.
    public void MostraCrediti()
    {
        creditsCanvas.SetActive(true);
        creditsAnimator.gameObject.SetActive(true);
        creditsAnimator.Play("Credits Animation"); // Cambia "NomeDellaTuaAnimazione" con il nome dell'animazione che vuoi far partire.
        
        
    }

    // Funzione per nascondere i crediti.
    public void NascondiCrediti()
    {
        creditsCanvas.SetActive(false);
        creditsAnimator.gameObject.SetActive(false);
    }
}
