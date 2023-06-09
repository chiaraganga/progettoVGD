using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Atena_Dialogue : MonoBehaviour
{
    public GameObject dialogPanel;
    private GameObject Atena;
    public TMP_Text dialogueText;
    public string[] dialogo;
    private bool isWriting = false;

    private bool isDialogActive = false;
    private bool isFirstDialog = true;
    private bool isPlayerClose = false;

    private int index;
    public float velocityword;

    public float initialMessageDuration = 5f;
    public float delayBetweenMessages = 7f;

    public GameObject objectToShow;

    public QuizManager quizManager;

    private bool isFirstMessageShown = false;
    private bool isLastMessage = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        Atena = GameObject.FindGameObjectWithTag("Atena");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            if (!isDialogActive)
            {
                if (isFirstDialog)
                {
                    StartInitialDialog(dialogo[index]);
                    isFirstDialog = false;
                    Invoke("ShowNextMessage", initialMessageDuration);
                }
                else if (isFirstMessageShown)
                {
                    Invoke("ShowNextMessage", initialMessageDuration);
                }
            }
            else if (isWriting)
            {
                CompleteWriting();
                Invoke("StartNextMessage", delayBetweenMessages);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
        }
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
        isDialogActive = true;
        dialogPanel.SetActive(true);
        StartWriting(message);
    }

    private void EndDialog()
    {
        isDialogActive = false;
        if (isLastMessage)
        {
            dialogPanel.SetActive(false);
        }
        index = 0;

        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
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
        isWriting = false;
    }

    private IEnumerator WriteText(string message)
    {
        isWriting = true;

        for (int i = 0; i < message.Length; i++)
        {
            dialogueText.text += message[i];
            yield return new WaitForSeconds(velocityword);
        }

        yield return new WaitForSeconds(0.5f);

        isWriting = false;
    }

    private void ShowNextMessage()
    {
        index++;
        if (index < dialogo.Length)
        {
            StartDialog(dialogo[index]);
            initialMessageDuration = Time.time + velocityword * dialogo[index].Length;
            isFirstMessageShown = true;
        }
        else
        {
            isLastMessage = true;
            Invoke("EndDialog", 5f); // Chiude il dialogo dopo 5 secondi
        }
    }

    private void StartNextMessage()
    {
        ShowNextMessage();
    }

    private void QuizStartDialogue()
    {
        Debug.Log("funziona"); //testing
    }

    private void QuizEndDialogue()
    {
        if (quizManager.score == 3)
        {
            Debug.Log("funziona anche questo"); //testing
        }
    }
}
