using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualiser : MonoBehaviour
{
    public MeshRenderer cube;
    public Texture[] textures;

    [Range(3,18)]
    public int min = 3;
    [Range(3, 18)]
    public int max = 18;

    public float spacing = 1.2f;

    GameObject[] cubes;

    public int numInRange;

    int GetSum(int i, int j, int k)
    { 
        // add 3 because i = [0.5]
        return i + j + k + 3; 
    }

    // Start is called before the first frame update
    void Start()
    {
        cubes = new GameObject[216];

        // create cubes
        for (int i = 0; i < 6; i++)
            for (int j = 0; j < 6; j++)
                for (int k = 0; k < 6; k++)
                {
                    MeshRenderer mesh = Instantiate(cube, transform);
                    mesh.transform.localPosition = new Vector3(i*spacing, j*spacing, k*spacing);
                    int sum = GetSum(i,j,k);
                    mesh.material.SetTexture("_MainTex", textures[sum]);

                    int index = 6 * (6 * i + j) + k;
                    cubes[index] = mesh.gameObject;
                }

    }

    int oldMin, oldMax;

    // Update is called once per frame
    void Update()
    {
        if (oldMin != min)
            if (max < min)
                max = min;
        if (oldMax != max)
            if (min > max)
                min = max;

        oldMin = min;
        oldMax = max;

        numInRange = 0;
        for (int i = 0; i < 6; i++)
            for (int j = 0; j < 6; j++)
                for (int k = 0; k < 6; k++)
                {
                    int index = 6 * (6 * i + j) + k;
                    int sum = GetSum(i, j, k);
                    cubes[index].SetActive(sum >= min && sum <= max);
                    if (sum >= min && sum <= max)
                        numInRange++;
                }
    }
}
