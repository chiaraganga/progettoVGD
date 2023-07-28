using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsAnimator : MonoBehaviour
{
    public Animator creditsAnimator;

    void Start()
    {
        // Supponendo che "Credits Animation" sia il nome della tua animazione
        creditsAnimator.Play("Credits Animation");
    }
}
