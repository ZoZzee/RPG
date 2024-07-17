using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int coins = 0;

    public float health;
    public float healthMax;
    public Image healthImage;

    public float speedWalking = 5;
    public float speedRunning = 10;
    public float maxSpeed = 10;
    private float speed = 5;

    private float gravity = 13;
    public float space = 100;

    public bool isGround = true;
    public bool isAir = true;
    private SpriteRenderer spritsRenderer;
    private Rigidbody2D rigidBody2d;
    public Vector3 velociti;
    private Animator animator;

    public TMP_Text coinsText;

    void Awake()
    {
        spritsRenderer = GetComponent<SpriteRenderer>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        // Пришвидшення
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = speedRunning;
        }
        else
        {
            speed = speedWalking;
        }
        //Упарвління гравцем
        rigidBody2d.velocity = new Vector2 (Input.GetAxis("Horizontal") * speed, rigidBody2d.velocity.y);
        //Гравітація
        if(rigidBody2d.velocity.y < 0)
        {
            if(rigidBody2d.velocity.y <= -maxSpeed)
            {
                rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, -maxSpeed);
            }
            rigidBody2d.AddForce(-transform.up * gravity);

        }
    }
    private void Update()
    {
        velociti = rigidBody2d.velocity;
        // Прижок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround == true)
            {
                rigidBody2d.AddForce(transform.up * space, ForceMode2D.Impulse);
            }
            else if (isAir == true)
            {
                rigidBody2d.AddForce(transform.up * space, ForceMode2D.Impulse);
                isAir = false;
            }
        }
        // Поворот персонажа
        if (Input.GetKey(KeyCode.A))
        {
            spritsRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spritsRenderer.flipX = true;
        }
        // робота з анімацією бігу
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Run",true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

    }

    public void Damage( float newDamage)
    {
        health -= newDamage;
        healthImage.fillAmount = health / healthMax;
        
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void LateUpdate()
    {
        //Робота з анімацією прижка
        animator.SetBool("isGround", isGround);

        //Робота з текстом монетки
        coinsText.text = coins.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            isAir = true;  
        }
        else if(collision.gameObject.GetComponent<EnemyOne>() && collision.isTrigger == false)
        {
            collision.gameObject.GetComponent<EnemyOne>().Damage();
            rigidBody2d.velocity = new Vector2(rigidBody2d.velocity.x, 0);
            rigidBody2d.AddForce(transform.up * space, ForceMode2D.Impulse);
        }else if(collision.gameObject.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }
}
