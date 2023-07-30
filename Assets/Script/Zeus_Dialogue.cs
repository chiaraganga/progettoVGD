using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Zeus_Dialogue : MonoBehaviour
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
    private CharacterController player;


    void Start()
    {
        dialogPanel.SetActive(false);
        Zeus = GameObject.FindGameObjectWithTag("Zeus");
        player = FindObjectOfType<CharacterController>();
        
        
    }

    void Update()
    {
        //Controlla se Zeus è attivo
        if (isFirstDialog && Zeus != null && Zeus.activeSelf)
        {
            //Mostra il primo messaggio
            StartInitialDialog("Ercole! Grazie al cielo hai aggiustato la mia statua! Avvicinati, ti devo parlare.");
            isFirstDialog = false;
            Invoke("EndInitialDialog", initialMessageDuration);
            
        }

        if (isDialogActive)
        {
            //Blocca il movimento del player quando il dialogo è attivo
            player.enabled = false;
            
            if (isWriting)
            {
                if (Time.time >= initialMessageDuration)
                {
                    CompleteWriting();
                    Invoke("StartNextMessage", delayBetweenMessages);
                    
                }
            }
        }
        //Controllo se il player è vicino e condizione per premere T per iniziare il dialogo
        else if (isPlayerClose && Input.GetKeyDown(KeyCode.T) || (isPlayerClose && Input.GetKey(KeyCode.Joystick1Button1)))
        {
            if (index < dialogo.Length)
            {
                StartDialog(dialogo[index]);
                initialMessageDuration = Time.time + velocityword * dialogo[index].Length;
                
            }
        }
        else
        {
            player.enabled = true;
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

    //Funzione per iniziare il dialogo in riferimento al messaggio iniziale
    private void StartInitialDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogueText.text = message;
        
    }

    //Funzione per finire il dialogo in riferimento al messaggio iniziale
    private void EndInitialDialog()
    {
        dialogPanel.SetActive(false);
        
    }

    //Funzione per iniziare il dialogo
    private void StartDialog(string message)
    {
        isDialogActive = true;
        dialogPanel.SetActive(true);
        StartWriting(message);
        
    }

    //Funzione per finire il dialogo
    private void EndDialog()
    {
        isDialogActive = false;
        dialogPanel.SetActive(false);
        index = 0;
        
        //Mostra la spada
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
        }
    }

    //Funzione per iniziare a scrivere
    private void StartWriting(string message)
    {
        dialogueText.text = "";
        StartCoroutine(WriteText(message));
    }

    //Funzione che completa la scrittura
    private void CompleteWriting()
    {
        StopAllCoroutines();
        dialogueText.text = dialogo[index];
        isWriting = false;
    }

    //Funzione che scrive il messaggio parola per parola
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

    //Funzione per mostrare il messaggio successivo
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