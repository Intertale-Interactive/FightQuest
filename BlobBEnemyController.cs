using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBEnemyController : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Rigidbody2D rb;
    public RaycastHit hit;

    public RaycastBlob raycastBlob;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(-1.5f, rb.velocity.y);
    }
}
