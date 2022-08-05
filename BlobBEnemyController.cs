using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BlobBEnemyController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject player;
    public GameObject enemy;
    public RaycastHit hit;
    public bool canAttack;
    public bool stop;
    public float direction;
    public float speed = 3f;
    public Slider playerSlider;

    public PlayerController playerController;

    void Start()
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        anim = enemy.GetComponent<Animator>();
        direction = -1.5f;

        canAttack = false;
        stop = false;
    }

    void Update()
    {
        FollowPlayer();

        VerifyCanAttack();
        Attack1();

        // Debug.Log("Stop = " + stop);
    }

    public void FollowPlayer()
    {
        float dist = Vector2.Distance(player.transform.position, enemy.transform.position);

        if (dist < 1.37f)
        {
            stop = true;
            rb.velocity = new Vector2(0, 0);
        } else if (dist > 1.37f)
        {
            stop = false;
        }

        if (!stop)
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

    public void VerifyCanAttack()
    {
        float dist1 = Vector2.Distance(player.transform.position, enemy.transform.position);
        // Debug.Log(dist1);

        if (dist1 < 1.37f)
        {
            canAttack = true;
        } else if (dist1 > 1.37f)
        {
            canAttack = false;
        }
    }

    public void MakeDamage()
    {
        playerSlider.value -= 5;
    }
}