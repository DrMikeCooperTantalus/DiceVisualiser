using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CumulativeTest : MonoBehaviour
{
    [Header("User Settings")]
    [Tooltip("Chance of getting a success on a fresh roll")]
    public float baseChance;
    [Tooltip("Increase of chance of success after a fail")]
    public float deltaChance;

    [Header("Internals")]
    public float currentChance;
    public bool lastRoll;
    public float average;

    const int numBins = 10;

    [Tooltip("Out of 1,000 players, how many got N rewards in 100 runs?")]
    public int[] bins = new int[numBins];

    [Tooltip("Out of 1,000 players, how many got N rewards in the first 50 runs?")]
    public int[] bins50 = new int[numBins];

    public Chart chart;
    public Chart chart50;

    // Start is called before the first frame update
    void Start()
    {
        currentChance = baseChance;
    }

    float lastBase = -1;
    float lastDelta = -1;

    // check to auto-update graph when we change values
    private void Update()
    {
        if (lastBase != baseChance || lastDelta !=deltaChance)
        {
            lastBase = baseChance;
            lastDelta = deltaChance;
            Test();
        }
    }

    [ContextMenu("Roll")]
    void Roll()
    {
        lastRoll = Random.Range(0.0f,100.0f) <= currentChance;
        if (lastRoll)
            currentChance = baseChance;
        else
            currentChance += deltaChance;
    }

    [ContextMenu("Get Average")]
    void Test()
    {
        average = 0;

        int totalRuns = 100;

        for (int i = 0; i < numBins; i++)
        {
            bins[i] = 0;
            bins50[i] = 0;
        }

        for (int i = 0; i < 1000; i++)
        {
            currentChance = baseChance;
            int count = 0;
            int count50 = 0;
            for (int j = 0; j < totalRuns; j++)
            {
                Roll();
                if (lastRoll)
                {
                    count++;
                    average += 1.0f;
                }
                if (j == 50)
                    count50 = count;
            }
            bins[Mathf.Min(count, numBins-1)]++;
            bins50[Mathf.Min(count50, numBins-1)]++;
        }
        // turn into a percentage
        average /= 1000.0f;

        if (chart)
            chart.SetValues(bins);
        if (chart50)
            chart50.SetValues(bins50);
    }

    [ContextMenu("Create Tables")]
    void MakeTables()
    {
        System.IO.StreamWriter f = new System.IO.StreamWriter("Tables.csv");
        System.IO.StreamWriter f0 = new System.IO.StreamWriter("P0.csv");
        System.IO.StreamWriter f50 = new System.IO.StreamWriter("P0_50.csv");

        for (int i=0; i< 21; i++)
        {
            for (int j=0; j<21; j++)
            {
                baseChance = i * 0.1f; // 0 to 2% in 0.1% increments
                deltaChance = j * 0.01f; // 0 to 0.2% in 0.01% increments
                Test();
                f.Write(average.ToString() + ",");
                f0.Write(bins[0].ToString() + ",");
                f50.Write(bins50[0].ToString() + ",");
            }
            f.Write("\n");
            f0.Write("\n");
            f50.Write("\n");
        }

        f.Close();
        f0.Close();
        f50.Close();
    }
}
