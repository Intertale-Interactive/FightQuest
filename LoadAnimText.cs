using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAnimText : MonoBehaviour
{
    public GameObject text;
    public GameObject image;

    private void Start()
    {
        image.SetActive(true);
        text.SetActive(false);

    }

    public void AfficheText()
    {
        image.SetActive(false);
        text.SetActive(true);
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1.5f);
        image.SetActive(true);
        text.SetActive(false);
    }
}
