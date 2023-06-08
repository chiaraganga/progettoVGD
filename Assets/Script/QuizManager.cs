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

    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;

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
    }

    public void correct()
    {
        // QnA.RemoveAll(currentQuestion);
        score += 1;
        generateQuestion();
        StartCoroutine(WaitForNext());
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        generateQuestion();
        StartCoroutine(WaitForNext());
    }
    IEnumerator WaitForNext()
    {
        yield return new WaitForSeconds(1);
        generateQuestion();
    }

    void generateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswer();

            // Rimuovi la domanda corrente dalla lista QnA
            QnA.RemoveAt(currentQuestion);
        }
        else
        {
            Debug.Log("No Question");
            GameOver();
        }
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

            ColorBlock colors = options[i].GetComponent<Button>().colors;
            colors.normalColor = options[i].GetComponent<AnswersScript>().startColor;
            options[i].GetComponent<Button>().colors = colors;
        }
    }
}