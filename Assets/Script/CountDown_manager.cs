using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown_manager : MonoBehaviour
{
    public GameObject dialogPanel;
    public GameObject startGame;
    public GameObject endDisplay;
    public GameObject go;
    public TMP_Text dialogueText;
    public string[] dialogo;
    public float velocityword;

    public int time_to_start;
    public int time_to_end;
    public Text start_display;
    public Text game_display;
    public Text end_display;

    private CharacterController player;
    

    private bool isWriting = false;
    private bool isDialogActive = false;
    private bool isCountdownActive = false;
    public bool finish = false;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        player = FindObjectOfType<CharacterController>();
        player.enabled = false;
       

        StartCoroutine(StartLevelRoutine());
    }

    private IEnumerator StartLevelRoutine()
    {
        // Avvia il dialogo
        yield return StartCoroutine(DialogueRoutine());

        // Avvia il conto alla rovescia
        yield return StartCoroutine(CountdownRoutine());

        // Abilita il personaggio e l'animazione
        player.enabled = true;
        

        // Nascondi il pannello di avvio
        start_display.gameObject.SetActive(false);

        // Avvia il conto alla rovescia del gioco
        StartCoroutine(GameCountdownRoutine());
    }

    private IEnumerator DialogueRoutine()
    {
        isDialogActive = true;
        dialogPanel.SetActive(true);
        game_display.gameObject.SetActive(false);
        startGame.SetActive(false);
        go.SetActive(false);
        endDisplay.SetActive(false);




        foreach (string message in dialogo)
        {
            StartWriting(message);
            yield return new WaitUntil(() => !isWriting);
            yield return new WaitForSeconds(1f);
        }

        isDialogActive = false;
        dialogPanel.SetActive(false);
        game_display.gameObject.SetActive(true);
        startGame.SetActive(true);
        go.SetActive(false);
        endDisplay.SetActive(false);
    }

    private IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(1f); // Aggiungi un piccolo ritardo prima di avviare il conto alla rovescia

        isCountdownActive = true;
        start_display.gameObject.SetActive(true);

        while (time_to_start > 0)
        {
            start_display.text = time_to_start.ToString();
            yield return new WaitForSeconds(1f);
            time_to_start--;
        }

        start_display.text = "RUN!";
        yield return new WaitForSeconds(1f);
        start_display.gameObject.SetActive(false);

        isCountdownActive = false;
    }

    public IEnumerator GameCountdownRoutine()
    {
        game_display.gameObject.SetActive(true);
        if (finish == false)
        {
            while (time_to_end > 0)
            {

                game_display.text = time_to_end.ToString();
                yield return new WaitForSeconds(1f);
                time_to_end--;
            }

            game_display.gameObject.SetActive(false);
            end_display.gameObject.SetActive(true);
            end_display.text = "GAME OVER!";
            go.SetActive(true);

            player.enabled = false;
        }
        
    }

    private void StartWriting(string message)
    {
        dialogueText.text = "";
        StartCoroutine(WriteTextRoutine(message));
    }


    private IEnumerator WriteTextRoutine(string message)
    {
        dialogueText.text = "";
        isWriting = true;

        for (int i = 0; i < message.Length; i++)
        {
            dialogueText.text += message[i];
            yield return new WaitForSeconds(velocityword);
        }

        isWriting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDialogActive && isWriting)
            {
                // Salta la scrittura del dialogo corrente
                CompleteWriting();
            }
            else if (!isCountdownActive)
            {
                // Salta il conto alla rovescia di avvio
                StopAllCoroutines();
                StartCoroutine(GameCountdownRoutine());
            }
        }
        if(finish==true)
        {
            StopAllCoroutines();
        }
    }

    private void CompleteWriting()
    {
        if (isWriting)
        {
            StopAllCoroutines();
            dialogueText.text = dialogo[index];
            isWriting = false;
        }
    }

   
}