using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public List<QuestionHandler> QnA;
    public GameObject[] options;
    public int currentQuestion;
    public GameObject Quizpanel;
    public GameObject GoPanel;

    private Coroutine timerCoroutine;
    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;
    public bool t;
    

    // private bool quizCompleted = false;

    public int score;

    int totalQuestions = 0;
    public int trigger_counter = 0;

    private void Start()
    {
        totalQuestions = QnA.Count;

        GoPanel.SetActive(false);
        generateQuestion();


        trigger_counter++;
        if (trigger_counter != 1)
        {
            QnA.RemoveAt(currentQuestion);
        }
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


    public void retry()
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Quizpanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions;
    }

    public void wrong()
    {
        //Risposta sbagliata
        // QnA.RemoveAll(currentQuestion);
        // generateQuestion();
        StartTimer();
    }

    void generateQuestion()
    {
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
        yield return new WaitForSeconds(2f);

        // Azioni da eseguire al termine del timer
        Debug.Log("Timer completato!");

        // Resetta la variabile del riferimento alla coroutine
        timerCoroutine = null;
        // t = true;
        generateQuestion();
    }

    void SetAnswer()
    {
      //  t = false;
      //  StartTimer();
       // while (!t) {

       // }

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswersScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswersScript>().isCorrect = true;
            }

            ColorBlock colors = options[i].GetComponent<Button>().colors;
            colors.normalColor = options[i].GetComponent<AnswersScript>().startColor;
            options[i].GetComponent<Button>().colors = colors;

            // Ripristina anche il colore selezionato
            colors = options[i].GetComponent<Button>().colors;
            colors.selectedColor = colors.normalColor;
            options[i].GetComponent<Button>().colors = colors;

            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswersScript>().startColor;
        }
    }

}