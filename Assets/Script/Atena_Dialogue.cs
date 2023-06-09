using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Atena_Dialogue : MonoBehaviour
{
    public GameObject dialogPanel;
    private GameObject Atena;
    public TMP_Text dialogueText;
    public string[] dialogo;
    private bool isWriting = false;

    public QuizManager quizManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogPanel.SetActive(false);
        Atena = GameObject.FindGameObjectWithTag("Atena");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void QuizStartDialogue()
    {
        Debug.Log("funziona"); //testing
    }
    private void QuizEndDialogue()
    {
        if (quizManager.score == 3)
        {
            Debug.Log("funziona anche questo"); //testing
        }
    }
}
