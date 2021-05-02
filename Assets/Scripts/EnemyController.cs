using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public int element;
    public int damage;
    public ParticleSystem explosionParticle;
    public AudioClip hitAudio;

    private GameObject player;
    private PlayerController playerController;
    Vector3 targetPosition;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver)
        {
            targetPosition = player.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Destroy(transform.gameObject);

            // Element matchups
            if (element == playerController.element)
            {
                // Heal if getting hit by matching element
                playerController.AddHealth(damage);
            }
            else if (playerController.element > 0)
            {
                // Take double damage if not neutral element taking a mismatched attack
                playerController.TakeDamage(damage * 2);
            }
            else
            {
                // Take 1 damage when neutral element
                playerController.TakeDamage(damage);
            }
        }
    }

    private void OnMouseDown()
    {
        gameManager.IncrementScore();
        AudioSource.PlayClipAtPoint(hitAudio, gameManager.cameraPosition.position);
        GameObject explosion = (GameObject)Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation).gameObject;
        Destroy(explosion, 3.0f);
        Destroy(gameObject);
    }
}
