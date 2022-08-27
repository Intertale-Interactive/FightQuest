using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BlobGController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Animator playerAnim;
    public GameObject player;
    public GameObject enemy;
    public GameObject faceLeft;
    public GameObject faceRight;
    public Transform faceLeftPos;
    public Transform faceRightPos;
    public RaycastHit hit;
    public bool canAttack;
    public bool stop;
    public bool test;
    public bool test2;
    public bool isFacingRight;
    public float direction;
    public float speed = 3f;
    public Slider playerSlider;
    public RaycastBlob raycastBlob;
    public Vector3 reverseVector1;
    public Vector3 reverseVector2;
    public Slider blobSlider;

    public PlayerController playerController;

    void Start()
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        anim = enemy.GetComponent<Animator>();
        direction = -1.5f;

        canAttack = false;
        stop = false;
        isFacingRight = false;
        test = false;
        test2 = false;
    }

    void Update()
    {
        FollowPlayer();
        RotateRays();
        Attack1();
        CheckIfDie();

        Test();

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

        if (distance < 1.45f)
        {
            test = true;
        } else if (distance > 1.45f)
        {
            test = false;
        }

        Debug.Log(test);
    }

    public void CheckIfDie()
    {
        if (blobSlider.value <= 0)
        {
            anim.Play("Idle");
            anim.SetBool("canAttack", false);
            StartCoroutine(StopAttack());
        }
    }

    public void Test()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(faceLeft.transform.position, Vector2.left, .5f);
        Debug.DrawRay(faceLeft.transform.position, Vector2.left * .5f, Color.red);
        RaycastHit2D hit2 = Physics2D.Raycast(faceRight.transform.position, Vector2.right, .5f);
        Debug.DrawRay(faceRight.transform.position, Vector2.right * .5f, Color.red);

        if (hit1.collider != null)
        {
            if (hit1.collider.gameObject.tag == "Player")
            {
                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.tag == "Player")
            {
                enemy.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        } else if (hit1.collider == null && hit2.collider == null)
        {
            if (direction == 1.5f)
            {
                enemy.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if (direction == -1.5f)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void FollowPlayer()
    {
        float dist = Vector2.Distance(player.transform.position, enemy.transform.position);

        if (dist < 1.45f && test2 == false)
        {
            stop = true;
            canAttack = true;
            rb.velocity = new Vector2(0, 0);
        } else if (dist > 1.45f && test2 == false)
        {
            stop = false;
            canAttack = false;
        } else if (dist < 1.45f && test2 == true)
        {
            stop = false;
            canAttack = true;
            rb.velocity = new Vector2(20, 8);
            test2 = false;
        } else if (dist > 1.45f && test2 == true)
        {
            stop = false;
            canAttack = false;
        }

        if (!stop && test2 == false)
        {
            if (playerController.hadToFollow == false)
            {
                rb.velocity = new Vector2(direction, rb.velocity.y);
            } 
            else if (playerController.hadToFollow == true) 
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }  
    }

    public void RotateRays()
    {
        if (isFacingRight)
        {
            faceRightPos.transform.position = enemy.transform.position + reverseVector1;
            faceLeftPos.transform.position = enemy.transform.position + reverseVector2;
        } else if (!isFacingRight)
        {
            faceRightPos.transform.position = enemy.transform.position + reverseVector1;
            faceLeftPos.transform.position = enemy.transform.position + reverseVector2;
        }
    }

    public void CheckFacingRight()
    {
        if (enemy.transform.rotation == Quaternion.Euler(0, -180, 0))
        {
            isFacingRight = true;
        } else if (enemy.transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            isFacingRight = false;
        }
    }

    public void Attack1()
    {
        if (canAttack == true)
        {
            anim.Play("Attack1");
            anim.SetBool("canAttack", true);
        } else if (canAttack == false)
        {
            anim.Play("Idle");
            anim.SetBool("canAttack", false);
        }
    }

    public void MakeDamage()
    {
        playerSlider.value -= 5;
    }

    public void AttackBlob()
    {
        // float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

        // if (distance < 1.45f)
        // {
        //     playerAnim.SetBool("attackBlob", true);
        //     playerAnim.Play("AttackBlob");
        // } else if (distance > 1.45f)
        // {
        //     playerAnim.SetBool("attackBlob", false);
        //     playerAnim.Play("idle");
        // }
        if (test == true)
        {
            test2 = true;
            DamageToBlob();
            Debug.Log(test2);
        } else if (test == false)
        {
            test2 = false;
            Debug.Log(test2);
        }
    }

    public void DamageToBlob()
    {
        blobSlider.value -= 10;
    }

    IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(3);
        Destroy(enemy);
    }
}