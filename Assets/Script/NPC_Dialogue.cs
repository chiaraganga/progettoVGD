using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_Dialogue : MonoBehaviour
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

    public float initialMessageDuration = 5f;
    public float delayBetweenMessages = 7f;

    public GameObject objectToShow;


    void Start()
    {
        dialogPanel.SetActive(false);
        Zeus = GameObject.FindGameObjectWithTag("Zeus");
    }

    void Update()
    {
        if (isFirstDialog && Zeus != null && Zeus.activeSelf)
        {
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
                    Invoke("StartNextMessage", delayBetweenMessages);
                }
            }
        }
        else if (isPlayerClose && Input.GetKeyDown(KeyCode.E) || (isPlayerClose && Input.GetKey(KeyCode.Joystick1Button1)))
        {
            if (index < dialogo.Length)
            {
                StartDialog(dialogo[index]);
                initialMessageDuration = Time.time + velocityword * dialogo[index].Length;
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
        dialogPanel.SetActive(false);
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

    private void StartNextMessage()
    {
        index++;
        if (index < dialogo.Length)
        {
            StartDialog(dialogo[index]);
            initialMessageDuration = Time.time + velocityword * dialogo[index].Length;
        }
        else
        {
            EndDialog();
        }
    }
}