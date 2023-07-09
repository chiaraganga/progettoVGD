using UnityEngine;

public class ControllerDebug : MonoBehaviour
{
    void Update()
    {
        // Controlla tutti i possibili pulsanti del joystick (da 0 a 19)
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKey("joystick button " + i))
            {
                Debug.Log("Joystick button " + i + " is pressed.");
            }
        }
    }
}
