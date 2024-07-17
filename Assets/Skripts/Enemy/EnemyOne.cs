using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyOne : MonoBehaviour
{
    private float speed;
    public float speedAttack;
    public float speedIdle;
    public bool alive = true;

    public Transform[] points;
    private int currentPoint;
    private int vectorMuve = 1;

    public bool isCircle;

    private Transform target;
    private Vector3 targetPosition;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    private bool culdown;
    public float timeCuldown;
    public float damage;
    public float culdownUp;

    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        if (alive == true)
        {
        if (target != null)
        {
            if (culdown == true)
            {
                targetPosition = target.position;
                targetPosition.y += culdownUp;
                speed = speedIdle;
            }
            else
            {
                targetPosition = target.position;
                speed = speedAttack;
            }
        }
        else
        {
            targetPosition = points[currentPoint].position;

            speed = speedIdle;

            if (transform.position == points[currentPoint].position)
            {
                currentPoint += vectorMuve;

                if (currentPoint == points.Length && isCircle == true)
                {
                    currentPoint = 0;
                }
                else if (currentPoint == points.Length && isCircle == false)
                {
                    vectorMuve *= -1;
                    currentPoint += vectorMuve;

                }
                if (currentPoint == -1)
                {
                    vectorMuve *= -1;
                    currentPoint += vectorMuve;
                }
            }

        }
        transform.position = Vector3.MoveTowards(transform.position,
                                                     targetPosition,
                                                     speed);
        }
    }

    private void Update()
    {
        if (transform.position.x > targetPosition.x )
        {
            spriteRenderer.flipX = false;
        }
        else if(transform.position.x < targetPosition.x)
        {
            spriteRenderer.flipX = true;
        }
    }

    IEnumerator culdownTimer()
    {
        yield return new WaitForSeconds(timeCuldown);

        culdown = false;
    }

    public void Damage()
    {
        alive = false;
        rigidbody2D.freezeRotation = false;
        rigidbody2D.gravityScale = 1;
        rigidbody2D.isKinematic = false;

        animator.SetBool("dead", true);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (alive == true)
            {
                culdown = true;
                StartCoroutine(culdownTimer());
                collision.gameObject.GetComponent<Player>().Damage(damage);
            }
        }
    }

}

