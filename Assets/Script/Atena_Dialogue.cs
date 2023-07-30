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
    private CharacterController player;

    private bool isDialogActive = false;
    private bool isFirstDialog = true;
    private bool isPlayerClose = false;

    private int index;
    public float velocityword;
    private bool isQuizCompleted = false;

    public float initialMessageDuration = 5f;
    public float delayBetweenMessages = 7f;
    public float panelDisableDuration = 5f; // Durata in secondi dopo l'ultimo messaggio prima di disabilitare il pannello

    public GameObject objectToShow;

    public QuizManager quizManager;

    public GameObject quizPanel;
    public CursorLockMode cursorLockMode;


    private bool isFirstMessageShown = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        Atena = GameObject.FindGameObjectWithTag("Atena");
        player = FindObjectOfType<CharacterController>();
        player.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Controllo se il giocatore è vicino e premo il tasto per avviare il dialogo
        if (isPlayerClose && (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Joystick1Button1)))
        {
            player.enabled = false;

            if (!isDialogActive)
            {
                //Disabilita il player quando inizia il dialogo
                player.enabled = false;

                if (isFirstDialog)
                {
                    StartInitialDialog(dialogo[index]);
                    isFirstDialog = false;
                    player.enabled = false;
                    Invoke("ShowNextMessage", initialMessageDuration);
                    
                }
                else if (isFirstMessageShown)
                {
                    player.enabled = false;
                    Invoke("ShowNextMessage", initialMessageDuration);
                    
                }
            }
            else if (isWriting)
            {
                //Se sto già scrivendo aspetto che finisca di scrivere
                CompleteWriting();
                player.enabled = false;
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

    //Funzione per iniziare il dialogo con il messaggio iniziale
    private void StartInitialDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogueText.text = message;
        player.enabled = false;
    }

    private void EndInitialDialog()
    {
        dialogPanel.SetActive(false);
    }

    //Funzione per iniziare il dialogo
    private void StartDialog(string message)
    {
        isDialogActive = true;
        dialogPanel.SetActive(true);
        player.enabled = false;

        //Mostra il messaggio una volta finito il quiz

        if (message == "Complimenti! Prendi lo scudo vicino al bracere")
        {
            dialogueText.text = message;
            player.enabled = true;
            Invoke("EndDialog", 5f); // Chiudi il dialogo dopo 5 secondi
            
        }
        else
        {
            StartWriting(message);
        }
    }


    private void EndDialog()
    {
        isDialogActive = false;
        dialogPanel.SetActive(false);
        index = 0;
        player.enabled = true;

        //if (objectToShow != null)
        //{
        //   objectToShow.SetActive(true);
        //}

        // Attiva il QuizPanel solo se isQuizCompleted è impostato su false
        if (!isQuizCompleted)
        {
            quizPanel.SetActive(true);
            player.enabled = false;
        }

        // Imposta la visibilità del cursore
        Cursor.lockState = cursorLockMode;
        Cursor.visible = true;
    }

    //Funzione che si occupa della scrittura
    private void StartWriting(string message)
    {
        dialogueText.text = "";
        player.enabled = false;
        StartCoroutine(WriteText(message));
        
    }

    private void CompleteWriting()
    {
        StopAllCoroutines();
        dialogueText.text = dialogo[index];
        isWriting = false;
        player.enabled = false;
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

            if (index == 2) // Controllo se è il secondo messaggio
            {
                Invoke("EndDialog", 5f); // Chiudi il dialogo dopo 5 secondi
            }
            else
            {
                float delay = index == 0 ? initialMessageDuration : delayBetweenMessages;
                Invoke("ShowNextMessage", delay); // Richiama dopo il delay appropriato
            }
        }
        else
        {
            player.enabled = true;
            EndDialog();
            
        }
    }


    private void StartNextMessage()
    {
        ShowNextMessage();
    }


    public void QuizEndDialogue()
    {
        //Messaggio di congratulazioni finito il quiz
        if (quizManager.score == 4)
        {
            StartDialog("Complimenti! Prendi lo scudo vicino al bracere");
            isQuizCompleted = true;
            quizPanel.SetActive(false);
            player.enabled = false;

            if (objectToShow != null)
            {
                objectToShow.SetActive(true);
            }
        }
    }
}
