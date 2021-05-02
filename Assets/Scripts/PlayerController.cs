using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int element;
    public int health;
    public Material[] materials;
    public GameObject elementCircle;
    public AudioClip healAudio;
    public AudioClip hurtAudio;
    public CameraShake cameraShake;

    private float xRange = 13f;
    private float zRange = 7f;
    private GameManager gameManager;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        element = 0;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameManager.UpdateHealth(health);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOn) { return; }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }

        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }

        Vector3 move = new Vector3(horizontalInput, 0, verticalInput);
        move = move.normalized * Time.deltaTime * speed;
        transform.Translate(move);

        // Controls - Change Element
        if (Input.GetKeyDown(KeyCode.Q))
        {
            element--;
            ChangeElement(element);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            element++;
            ChangeElement(element);
        }

        // Count clicks
        if (Input.GetMouseButtonDown(0))
        {
            gameManager.IncrementClicks();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
        health -= damageAmount;
        audioSource.clip = hurtAudio;
        audioSource.Play();

        if (health <= 0 || gameManager.wave >= 99)
        {
            health = 0;
            gameObject.SetActive(false);

            gameManager.EndGame();
        }

        gameManager.UpdateHealth(health);
    }

    public void AddHealth(int healAmount)
    {
        health += healAmount;
        gameManager.UpdateHealth(health);
        audioSource.clip = healAudio;
        audioSource.Play();
    }

    public void ChangeElement(int newElement)
    {
        //hide element circle
        elementCircle.SetActive(false);

        if (newElement >= materials.Length)
        {
            element = 0;
        }
        else if (newElement < 0)
        {
            element = materials.Length-1;
        }
        else
        {
            element = newElement;
        }

        //show element circle if non neutral element
        if (element > 0)
        {
            MeshRenderer meshRenderer = elementCircle.GetComponent<MeshRenderer>();
            meshRenderer.material = materials[element];
            elementCircle.SetActive(true);
        }

        gameManager.UpdateElement(element);
    }
}
