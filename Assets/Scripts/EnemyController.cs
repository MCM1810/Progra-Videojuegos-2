
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    private void Start()
    {
        mHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Hubo una colision
            mHealth -= maxHealth * 0.25f;
            Player.GetComponent<HeroController>().Recarga();
            healthbar.value -= 0.25f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }
           
        }
    }
}
