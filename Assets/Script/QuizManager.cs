using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    //Variabili
    public List<QuestionHandler> QnA;
    public GameObject[] options;
    public int currentQuestion;
    public GameObject Quizpanel;
    public GameObject GoPanel;
   
    private Coroutine timerCoroutine;
    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;

    private CharacterController player;

    public List<QuestionHandler> QnAReply = new List<QuestionHandler>();

    public int score;
  
    const int totalQuestions = 4; //Numero domande
    public int trigger_counter = 0;

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();

        // totalQuestions = QnA.Count;

        if (QnAReply.Count == 0) //inizializzare una sola volta
        for(int i=0; i<QnA.Count; i++) {
              //Scorro tutti gli elementi della lista
              QnAReply.Add(QnA[i].Clone());
                
        }

        //Disattivo il panello del punteggio
        GoPanel.SetActive(false); 
        score = 0;

        //Chiamo la funzione per generare le domande
        generateQuestion();

        //Visibile il cursore
        Cursor.visible = true;

        



    }

    public void correct()
    {
        //Aumento il punteggio
        score += 1;

        //Se il punteggio è 4 (quindi max) disattivo il quiz
        if (score >= 4)
        {
            //quizCompleted = true;
            Quizpanel.SetActive(false);
            GoPanel.SetActive(false);
            FindObjectOfType<Atena_Dialogue>().QuizEndDialogue();
        }
        else
        {
           // generateQuestion();

            //Chiamo startTimer
            StartTimer();
        }
    }

    public void wrong()
    {
        //Risposta sbagliata
        // QnA.RemoveAll(currentQuestion);
        // generateQuestion();
        StartTimer();
    }


    public void retry()
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //Richiamo la lista delle domande per effettuare il retry
        for (int i = 0; i < QnAReply.Count; i++)
        {
            QnA.Add(QnAReply[i].Clone());
        }
        
        Quizpanel.SetActive(true);
        Start();

    }

    //Funzione per stampare il punteggio
    public void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;
        player.enabled = false;

    }

    //Funzione per generare una domanda
    void generateQuestion()
    {
        
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswer();
         // StartTimer();

            // Rimuovi la domanda corrente dalla lista QnA
            QnA.RemoveAt(currentQuestion);
            
        }
        else
        {
            
            GameOver();

            //Se il punteggio è 4 quindi max, disattiva il pannello
            if (score == 4)
            {
                Quizpanel.SetActive(false);
                GoPanel.SetActive(false);

            }

        }

    }

   public void StartTimer()
   {
        /* Controllo che la Corutine non sia nulla
         * Se la variabile "timerCoroutine" non è nulla,
         * viene chiamato StopCoroutine per interrompere la coroutine corrente.
         * Questo è necessario per evitare l'esecuzione simultanea
         * di più coroutine che possono causare conflitti o errori.
         */

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(TimerCoroutine());

   }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1f);

        // Azioni da eseguire al termine del timer
       // Debug.Log("Timer completato!");

        // Resetta la variabile del riferimento alla coroutine
        timerCoroutine = null;
     
        generateQuestion();
    }

    /*
     Viene impostato "isCorrect" dell'oggetto "AnswersScript" associato all'elemento
     su "false". Questo indica che la risposta non è corretta di default.
     Viene impostato il testo della rispettiva risposta nell'oggetto "QnA" relativo
     alla domanda corrente. La variabile "currentQuestion" rappresenta l'indice
     della domanda corrente nell'array "QnA".
     Se l'indice dell'opzione corrente più uno è uguale alla risposta corretta di
     "QnA" relativo alla domanda corrente, allora "isCorrect" dell'oggetto = "true".
     Questo indica che è la risposta corretta.
     */

    void SetAnswer()
    {
        // Scorre gli elementi dell'array
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswersScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswersScript>().isCorrect = true;
            }

            /* Il colore dell'immagine dell'elemento viene impostato sul colore di partenza */

            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswersScript>().startColor;
        }
    }

}