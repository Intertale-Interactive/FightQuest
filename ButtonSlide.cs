using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSlide : MonoBehaviour
{
    public Transform slider;

    public void SlideToSet()
    {
        slider.transform.position = new Vector2(862, slider.position.y);
    }

    public void SlideToRemciement()
    {
        slider.transform.position = new Vector2(1505, slider.position.y);
    }

    public void Update()
    {
        Debug.Log(slider.transform.position);
    }
}
