using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToSetStart : MonoBehaviour
{
    public Slider slider;
    public GameObject settingsPanel;

    // Update is called once per frame
    void Update()
    {
        if (slider.value >= 0.427)
        {
            settingsPanel.SetActive(true);
        }

        if (settingsPanel.activeSelf == true)
        {
            StartCoroutine(waiter());
        }
    }

    IEnumerator waiter() 
    {
        yield return new WaitForSeconds(1f);
        slider.value = 0;
    }
}
