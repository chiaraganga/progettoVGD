using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NPC : MonoBehaviour
{
    public GameObject dialogPanel;
    private GameObject Zeus;
    public TMP_Text dialogueText;
    public string[] dialogo;
    private bool isWriting = false;

    private bool isDialogActive = false;
    private bool isFirstDialog = true;
    private bool isPlayerClose = false;

    private int index;
    public float velocityword;

    // Tempo di visualizzazione del messaggio "Ciao, ti devo parlare!"
    public float initialMessageDuration = 5f;

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
            StartDialog("Ciao, ti devo parlare!");
            isFirstDialog = false;
            Invoke("EndInitialDialog", initialMessageDuration);
        }

        if (isDialogActive)
        {
            if (Input.GetKeyDown(KeyCode.E) && isPlayerClose)
            {
                if (isWriting)
                {
                    CompleteWriting();
                }
                else
                {
                    if (index < dialogo.Length - 1)
                    {
                        index++;
                        StartWriting(dialogo[index]);
                    }
                    else
                    {
                        EndDialog();
                    }
                }
            }
        }
        else if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            StartDialog(dialogo[index]);
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

    // Metodo chiamato dopo il tempo di visualizzazione del messaggio iniziale
    private void EndInitialDialog()
    {
        EndDialog();
    }
}
