using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public List<QuestionHandler> QnA;
    public GameObject[] options;
    public int currentQuestion;
    public GameObject Quizpanel;
    public GameObject GoPanel;
   
    private Coroutine timerCoroutine;
    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;

    public List<QuestionHandler> QnAReply = new List<QuestionHandler>();

    public int score;
  
    const int totalQuestions = 3; //Numero domande
    public int trigger_counter = 0;

    private void Start()
    {
        Debug.Log("PARTE");
        // totalQuestions = QnA.Count;

        if (QnAReply.Count == 0) //inizializzare una sola volta
        for(int i=0; i<QnA.Count; i++) {
            QnAReply.Add(QnA[i].Clone());
                
            }

        GoPanel.SetActive(false);
        score = 0;
        generateQuestion();

        //Parte che non faceva funzionare 
       /* trigger_counter++;
        if (trigger_counter != 1)
        {
            QnA.RemoveAt(currentQuestion);
            trigger_counter = 0;
        }
       */
        //generateQuestion();
        Cursor.visible = true;
    
    }

    public void correct()
    {
        score += 1;

        if (score >= 3)
        {
            //quizCompleted = true;
            Quizpanel.SetActive(false);
            GoPanel.SetActive(false);
            FindObjectOfType<Atena_Dialogue>().QuizEndDialogue();
        }
        else
        {
           // generateQuestion();
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
        for (int i = 0; i < QnAReply.Count; i++)
        {
            QnA.Add(QnAReply[i].Clone());
        }
        Debug.Log(QnAReply.Count);
        Quizpanel.SetActive(true);
        Start();

    }

    public void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;

    }


    void generateQuestion()
    {
        Debug.Log(score);
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswer();
         //   StartTimer();

            // Rimuovi la domanda corrente dalla lista QnA
            QnA.RemoveAt(currentQuestion);
            
        }
        else
        {
            Debug.Log("No Question");
            GameOver();
            if (score == 3)
            {
                Quizpanel.SetActive(false);
                GoPanel.SetActive(false);
            

            }

        }

    }

   public void StartTimer()
    {
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
        Debug.Log("Timer completato!");

        // Resetta la variabile del riferimento alla coroutine
        timerCoroutine = null;
     
        generateQuestion();
    }

    void SetAnswer()
    {

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswersScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswersScript>().isCorrect = true;
            }

            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswersScript>().startColor;
        }
    }

}