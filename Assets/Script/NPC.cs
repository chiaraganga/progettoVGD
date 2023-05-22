using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.ConstrainedExecution;

public class NPC : MonoBehaviour
{
    //Variabili
    public GameObject dialogPanel;
    private GameObject Zeus;
    public TMP_Text dialogueText;
    public string[] dialogo;
    private bool isWriting = false;
    private bool isInitialDialogActive = false;

    private bool isDialogActive = false;
    private bool isFirstDialog = true;
    private bool isPlayerClose = false;

    private int index;
    public float velocityword;

    // Tempo di visualizzazione del messaggio iniziale
    public float initialMessageDuration = 5f;

    // Durata del secondo dialogo (3 secondi)
    public float secondDialogDuration = 3f;

    // Durata del terzo dialogo (7 secondi)
    public float thirdDialogDuration = 7f;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        Zeus = GameObject.FindGameObjectWithTag("Zeus");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstDialog && Zeus != null && Zeus.activeSelf)
        {
            // Mostra il messaggio iniziale e avvia il conteggio alla fine del quale scompare
            StartInitialDialog("Ercole! Grazie al cielo hai aggiustato la mia statua! Avvicinati, ti devo parlare.");
            isFirstDialog = false;
            Invoke("EndInitialDialog", initialMessageDuration);
        }

        if (isDialogActive) //controllo che il dialogo sia attivo
        {
            if (isWriting) //controllo se sta scrivendo
            {
                if (Time.time >= initialMessageDuration)
                {
                    //Se il tempo di gioco supera o è uguale alla durata del messaggio iniziale,
                    //completo la scrittura del testo.
                    CompleteWriting();
                }
            }
            else
            {
                if (Time.time >= initialMessageDuration)
                {
                    if (index < dialogo.Length - 1) //Se l'indice è inferiore al numero totale di messaggi incremento di 1
                    {
                        index++;
                        StartWriting(dialogo[index]); //Scrivo nuovo messaggio
                        initialMessageDuration = Time.time + velocityword * dialogo[index].Length;

                        if (index == dialogo.Length - 1)
                        {
                            //Se l'indice raggiunge l'ultimo messaggio nel dialogo, viene chiuso il dialogo.
                            Invoke("EndDialog", thirdDialogDuration);
                        }
                    }
                }
            }
        }
        else if (isPlayerClose && Input.GetKeyDown(KeyCode.E)) //Se il player e' vicino e ha premuto E
        {
            StartDialog(dialogo[index]);
            initialMessageDuration = Time.time + secondDialogDuration;
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
        isInitialDialogActive = true;
        dialogPanel.SetActive(true);
        dialogueText.text = message;
    }

private void EndInitialDialog()
    {
        isInitialDialogActive = false;
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
        dialogPanel.SetActive(false);
        index = 0;
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

        //Concatenazione
        for (int i = 0; i < message.Length; i++)
        {
            dialogueText.text += message[i];
            //Gestione della pausa
            yield return new WaitForSeconds(velocityword);
        }

        isWriting = false;
    }
}
