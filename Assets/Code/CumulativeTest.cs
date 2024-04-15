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

    [Tooltip("Out of 1,000 players, how many got N rewards in 100 runs?")]
    public int[] bins = new int[100];

    // Start is called before the first frame update
    void Start()
    {
        currentChance = baseChance;
    }


    [ContextMenu("Roll")]
    void Roll()
    {
        lastRoll = Random.Range(1.0f,100.0f) <= currentChance;
        if (lastRoll)
            currentChance = baseChance;
        else
            currentChance += deltaChance;
    }

    [ContextMenu("Get Average")]
    void Test()
    {
        average = 0;

        for (int i = 0; i < 100; i++)
            bins[i] = 0;

        for (int i = 0; i < 1000; i++)
        {
            currentChance = baseChance;
            int count = 0;
            for (int j = 0; j < 100; j++)
            {
                Roll();
                if (lastRoll)
                {
                    count++;
                    average += 1.0f;
                }
            }
            bins[count]++;
        }
        // turn into a percentage
        average /= 1000.0f;
    }

    [ContextMenu("Create Tables")]
    void MakeTables()
    {
        System.IO.StreamWriter f = new System.IO.StreamWriter("Tables.csv");
        System.IO.StreamWriter f0 = new System.IO.StreamWriter("P0.csv");

        for (int i=0; i< 20; i++)
        {
            for (int j=0; j<20; j++)
            {
                baseChance = i * 0.1f; // 0.1 to 2%
                deltaChance = j * 0.01f; // 0 to 1.9%
                Test();
                f.Write(average.ToString() + ",");
                f0.Write(bins[0].ToString() + ",");
            }
            f.Write("\n");
            f0.Write("\n");
        }

        f.Close();
        f0.Close();
    }
}
