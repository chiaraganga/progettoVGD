using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMP_Text dialogueText;
    public string[] dialogo;
    private bool isWriting = false;

    private bool isDialogActive = false;
    private bool isFirstDialog = true;

    private int index;
    public float velocityword;
    public bool playerIsClose;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialogPanel.activeInHierarchy)
            {
                nienteTesto();
            }
            else
            {
                dialogPanel.SetActive(true);
                isDialogActive = true;
                StartCoroutine(Scrittura());
            }
        }

        if (isDialogActive && !isWriting && Input.anyKeyDown)
        {
            NuovaLinea();
        }
    }

    public void nienteTesto()
    {
        dialogueText.text = "";
        index = 0;
        dialogPanel.SetActive(false);
        isDialogActive = false;
        isFirstDialog = true;
    }

    IEnumerator Scrittura()
    {
        isWriting = true;

        foreach (char letter in dialogo[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(velocityword);
        }

        isWriting = false;
    }

    public void NuovaLinea()
    {
        if (isWriting)
        {
            return;
        }

        if (isFirstDialog)
        {
            isFirstDialog = false;
            index = 1;
        }
        else
        {
            if (index < dialogo.Length - 1)
            {
                index++;
            }
            else
            {
                nienteTesto();
                return;
            }
        }

        dialogueText.text = "";
        StartCoroutine(Scrittura());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            nienteTesto();
        }
    }
}
