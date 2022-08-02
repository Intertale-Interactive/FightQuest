using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToTextConnect : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text text;

    void Update()
    {
        text.text = healthBar.value.ToString();
    }
}
