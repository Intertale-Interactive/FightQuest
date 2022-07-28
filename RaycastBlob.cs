using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBlob : MonoBehaviour
{
    public bool 

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.right * -1), 2f);

        Debug.DrawRay(transform.position, (transform.right * -1) * 2f, Color.red);

        if (hit.collider)
        {
            if (hit.collider.isTrigger){
                Debug.Log("Touché !");
            }
        }  
    }
}
