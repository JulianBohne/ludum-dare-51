using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public int spawnCount = 100;
    public int minDistance = 5;
    public int maxDistance = 50;

    [System.Serializable]
    public class Spawnable
    {
        public GameObject type;
        public float spawnrate;
    }

    public Spawnable[] items;

    private Spawnable[] summed;

    private void Awake()
    {
        calcSums();

        for(int i = 0; i < spawnCount; i++)
        {
            float angle = Random.Range(0, Mathf.PI * 2);
            float dist = Random.Range(minDistance, maxDistance);

            Vector3 position = new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0);

            Instantiate(getType(Random.Range(0f, 1f)), position, Quaternion.identity).transform.parent = transform;
        }
    }

    private void calcSums()
    {
        summed = new Spawnable[items.Length];
        float sum = 0;
        for(int i = 0; i < items.Length; i++)
        {
            sum += items[i].spawnrate;
            summed[i] = new Spawnable { spawnrate = sum, type = items[i].type };
        }
    }

    private GameObject getType(float rand)
    {
        rand *= summed[summed.Length - 1].spawnrate;
        foreach(Spawnable s in summed)
        {
            if (rand < s.spawnrate) return s.type;
        }
        return summed[summed.Length - 1].type;
    }
}
