using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Script per il dialogo di Phobos, inizia direttamente come inizia la scena.

public class General_Dialogue : MonoBehaviour
{
    public GameObject dialogPanel;
    private GameObject Phobos;
    public TMP_Text dialogueText;
    public string[] dialogo;

    private int index;
    public float velocityword;

    public float initialMessageDuration = 5f;
    public float delayBetweenMessages = 6f;
    public float panelDisableDuration = 5f; // Durata in secondi dopo l'ultimo messaggio prima di disabilitare il pannello

    public GameObject objectToShow;


    // Start is called before the first frame update
    void Awake()
    {
        dialogPanel.SetActive(false);
        Phobos = GameObject.FindGameObjectWithTag("Phobos");
        StartInitialDialog(dialogo[index]);
        Invoke("ShowNextMessage", initialMessageDuration);
    }

    private void StartInitialDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogueText.text = message;
    }

    private void EndInitialDialog()
    {
        dialogPanel.SetActive(false);
    }

    private void StartDialog(string message)
    {
        dialogPanel.SetActive(true);

        StartWriting(message);
    }

    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        index = 0;

        //if (objectToShow != null)
        //{
        //   objectToShow.SetActive(true);
        //}

        if (GameObject.FindGameObjectWithTag("Phobos"))
        {
            Phobos.SetActive(false);
        }
    }


    private void StartWriting(string message)
    {
        dialogueText.text = "";
        StartCoroutine(WriteText(message));
    }

    private void CompleteWriting()
    {
        StopAllCoroutines();
        dialogueText.text = dialogo[index];
    }

    private IEnumerator WriteText(string message)
    {

        for (int i = 0; i < message.Length; i++)
        {
            dialogueText.text += message[i];
            yield return new WaitForSeconds(velocityword);
        }

        yield return new WaitForSeconds(0.5f);

    }

    private void ShowNextMessage()
    {
        index++;

        if (index < dialogo.Length)
        {
            StartDialog(dialogo[index]);

            if (index == dialogo.Length - 1) // Controllo se è l'ultimo messaggio
            {
                Invoke("EndDialog", panelDisableDuration); // Chiudi il dialogo dopo il tempo specificato in "panelDisableDuration"
            }
            else
            {
                float delay = index == 0 ? initialMessageDuration : delayBetweenMessages;
                Invoke("ShowNextMessage", delay);
            }
        }
        else
        {
            EndDialog();
        }
    }

    private void StartNextMessage()
    {
        ShowNextMessage();
    }
}
