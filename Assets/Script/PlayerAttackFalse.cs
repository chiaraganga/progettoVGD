using UnityEngine;

public class PlayerAttackFalse : MonoBehaviour
{
    // Riferimento al controller del personaggio
    private Player_controller playerController;

    private void Start()
    {
        // Ottieni il riferimento al controller del personaggio
        playerController = GetComponent<Player_controller>();
    }

    // Questa funzione viene chiamata quando l'animazione di attacco è terminata
    public void EndAttack()
{
    if (gameObject.tag == "Player" && playerController != null)
    {
        playerController.isAttacking = false;
    }
}

}
