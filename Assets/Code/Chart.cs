using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class Chart : MonoBehaviour
{
    public ChartBar[] bars;
    // Start is called before the first frame update

    public TextMeshProUGUI average;

    void Start()
    {
        if (bars.Length == 0)
            bars = GetComponentsInChildren<ChartBar>();
    }

    public void SetValues(float[] values)
    {
        for (int i = 0; i < values.Length && i < bars.Length; i++)
            bars[i].SetValue(values[i]);
    }

    public void SetValues(int[] values)
    {
        float total = 0;
        float count = 0;

        for (int i = 0; i < values.Length && i < bars.Length; i++)
        {
            bars[i].SetValue(values[i]);
            total += i * values[i];
            count += values[i];
        }

        if (average)
            average.SetText((total/count).ToString("F2"));
    }
}
