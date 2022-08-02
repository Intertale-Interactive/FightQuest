using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBlob : MonoBehaviour
{
    public GameObject blobObject;
    public GameObject trigger1;
    public GameObject trigger2;
    public BlobBEnemyController blob;

    void Update()
    {
        Function();
    }

    public void Function()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.right * -1), .02f);

        Debug.DrawRay(transform.position, (transform.right * -1) * .02f, Color.red);

        if (hit.collider)
        {
            if (hit.collider.isTrigger && (trigger1.tag == "TriggerForBlob" || trigger2.tag == "TriggerForBlob")){
                // Debug.Log("Touché !");
                Flip();                
            }
        }  
    }

    public void Flip()
    {
        blobObject.transform.Rotate(0.0f, 180.0f, 0.0f);
        blob.direction = blob.direction * -1;
    }
}