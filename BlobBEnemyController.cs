using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBEnemyController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public RaycastHit hit;
    public float direction;
    public float speed = 3f;

    public PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = -1.5f;
    }

    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        if (playerController.hadToFollow == false)
        {
            rb.velocity = new Vector2(direction, rb.velocity.y);
        } else if (playerController.hadToFollow == true)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}