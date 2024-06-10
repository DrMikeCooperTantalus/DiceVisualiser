using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartBar : MonoBehaviour
{
    public TextMeshProUGUI number;
    public Image bar;

    // Update is called once per frame
    public void SetValue(float value)
    {
        (bar.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
        number.text = ((int)value).ToString();
    }
}
