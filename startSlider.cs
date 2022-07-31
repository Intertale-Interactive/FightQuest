using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class startSlider : MonoBehaviour
{
    public Slider slider;

    public GameObject GO1;
    public GameObject GO2;

    void Start()
    {
        StartCoroutine(coroutine());
    }

    IEnumerator coroutine()
    {
        // Change value
        slider.value = 1f;
        yield return new WaitForSeconds(.45f);
        slider.value = 3f;
        yield return new WaitForSeconds(.19f);
        slider.value = 4f;
        yield return new WaitForSeconds(.1f);
        slider.value = 7f;
        yield return new WaitForSeconds(.5f);
        if (slider.value == 7)
        {
            GO1.SetActive(false);
            GO2.SetActive(true);
        }
    }
}
