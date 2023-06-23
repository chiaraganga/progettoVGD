using UnityEngine;

public class EnemyPhaseController : MonoBehaviour
{
    public GameObject enemyPrefab;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            // Abilitiamo Ares
            enemyPrefab.gameObject.SetActive(true);
            enemyPrefab.transform.position = transform.position;

            // Istanzia il nemico di nome Ares al posto del cubo
            // Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            Destroy(this.gameObject); // Distrugge il cubo


        }
    }

}
