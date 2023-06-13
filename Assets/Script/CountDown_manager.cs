using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown_manager : MonoBehaviour
{
    private CharacterController player;
    private Animator anim;
    public int time_to_start;
    public int time_to_end;
    public Text start_display;
    public Text game_display;
    public Text end_display;


    private void Start()
    {
        game_display.gameObject.SetActive(false);
        end_display.gameObject.SetActive(false);
        player = FindObjectOfType<CharacterController>();
        player.enabled = false;
        anim = FindObjectOfType<Animator>();
        anim.GetComponent<Animator>();
        
        StartCoroutine(start_countdown());
        
            
            
        
        
        
    }
    IEnumerator start_countdown()
    {
        
        while (time_to_start > 0)
        {
            start_display.text = time_to_start.ToString();
            yield return new WaitForSeconds(1f);
            time_to_start--;
        }
        start_display.text = "RUN!";
        player.enabled = true;
        anim.enabled = true;

        yield return new WaitForSeconds(1f);
        start_display.gameObject.SetActive(false);
        StartCoroutine(game_countdown());



    }

    IEnumerator game_countdown()
    {
        game_display.gameObject.SetActive(true);
        
        while (time_to_end > 0)
        {
            game_display.text = time_to_end.ToString();
            yield return new WaitForSeconds(1f);
            time_to_end--;
        }
        game_display.gameObject.SetActive(false);
        end_display.gameObject.SetActive(true);
        end_display.text = "GAME OVER!";
        player.enabled = false;
        anim.enabled = false;


        yield return new WaitForSeconds(1f);

        

    }
}
