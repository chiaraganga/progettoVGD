using System.Collections;
using TMPro;
using UnityEngine;

public class Ares_Dialogue : MonoBehaviour
{
    public GameObject dialogPanel; // Riferimento al pannello del dialogo da attivare
    public TMP_Text dialogText; // Riferimento al componente di testo UI per mostrare i messaggi
    public float messageDelay = 5f; // Ritardo tra i messaggi (in secondi)
    public float letterDelay = 0.1f; // Ritardo tra le singole lettere (in secondi)

    private void Start()
    {
        // Cerca l'oggetto Ares nella scena
        GameObject ares = GameObject.Find("Ares");

        // Se Ares è presente nella scena, avvia il dialogo
        if (ares != null)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        // Attiva il pannello del dialogo
        dialogPanel.SetActive(true);

        // Avvia la visualizzazione dei messaggi
        StartCoroutine(DisplayMessages());
    }

    private IEnumerator DisplayMessages()
    {
        // Messaggi da mostrare
        string[] messages = { "NO! Tu non sei degno di entrare nell'Olimpo", "Ora dovrai vedertela con me!!" };

        foreach (string message in messages)
        {
            // Mostra il messaggio nel componente di testo UI lettera per lettera
            for (int i = 0; i <= message.Length; i++)
            {
                dialogText.text = message.Substring(0, i);
                yield return new WaitForSeconds(letterDelay);
            }

            // Aspetta per il ritardo tra i messaggi
            yield return new WaitForSeconds(messageDelay);
        }

        // Disattiva il pannello del dialogo dopo il completamento dei messaggi
        dialogPanel.SetActive(false);
    }
}
