using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
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

    // Durata del secondo dialogo dopo aver premuto E (4 secondi)
    public float secondDialogDuration = 3f;

    // Durata del terzo dialogo dopo aver premuto E (8 secondi)
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

        if (isDialogActive)
        {
            if (isWriting)
            {
                if (Time.time >= initialMessageDuration)
                {
                    CompleteWriting();
                }
            }
            else
            {
                if (Time.time >= initialMessageDuration)
                {
                    if (index < dialogo.Length - 1)
                    {
                        index++;
                        StartWriting(dialogo[index]);
                        initialMessageDuration = Time.time + velocityword * dialogo[index].Length;

                        if (index == dialogo.Length - 1)
                        {
                            Invoke("EndDialog", thirdDialogDuration);
                        }
                    }
                }
            }
        }
        else if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
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

        for (int i = 0; i < message.Length; i++)
        {
            dialogueText.text += message[i];
            yield return new WaitForSeconds(velocityword);
        }

        isWriting = false;
    }
}
