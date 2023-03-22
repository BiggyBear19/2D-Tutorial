using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public int sceneBuildIndex;

    public float speed;

    public GameObject winMessage;

    public GameObject loseMessage;

    public float jumpForce;

    public Text score;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;

    public Text lives;

    public int liveCount = 3;

    public AudioClip backgroundMusic;

    public AudioClip victoryMusic;

    public AudioSource musicSource;






    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = Scores.score.ToString();
        lives.text = "Lives: " + liveCount;
        winMessage.SetActive(false);
        loseMessage.SetActive(false);
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop= true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Scores.score += 1;
            score.text = Scores.score.ToString();
            Destroy(collision.collider.gameObject);
            if (Scores.score == 4 ) 
            {
                SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
                liveCount= 3;
                
            }
            if (Scores.score == 8)
            {
                winMessage.SetActive(true);
                musicSource.clip = victoryMusic;
                musicSource.Play();
                musicSource.loop= false;
                Scores.score = 0;
                liveCount+= 99;

            }
        }

        if (collision.collider.tag == "Enemy")
        {
            liveCount-= 1;
            lives.text = "Lives: " + liveCount;
            Destroy(collision.collider.gameObject);
            if (liveCount == 0)
            {
                loseMessage.SetActive(true);
                gameObject.SetActive(false);
            }

            
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround) 
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
  

}