using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswersScript : MonoBehaviour
{
    //Variabili
    public bool isCorrect = false;
    public QuizManager quizManager;

    public Color startColor;

    /* Il colore dell'immagine associata all'oggetto viene assegnato alla 
     * variabile "startColor". Questo viene fatto per memorizzare il colore 
     * iniziale dell'oggetto prima che venga modificato successivamente durante 
     * la selezione della risposta.*/

    public void Start()
    {
        startColor = GetComponent<Image>().color;
    }

    public void Answer()
    {
        if (isCorrect)
        {
            //Cambio il colore se è corretta in verde
            GetComponent<Image>().color = Color.green;
            Debug.Log("Correct Answer");
            quizManager.correct();
        }
        else
        {
            //Cambio colore se è sbagliata in rosso
            GetComponent<Image>().color = Color.red;
            Debug.Log("Bad Answer");
            quizManager.wrong();
        }
    }
}
