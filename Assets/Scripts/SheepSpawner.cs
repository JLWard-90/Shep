using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject sheepPrefab;
    [SerializeField]
    Vector2 sheepXRange;
    [SerializeField]
    Vector2 sheepZRange;
    public int nSheep;
    private void Start() 
    {
        SpawnSheepAtRandom(nSheep);
    }
    void SpawnSheepAtRandom(int nSheep)
    {
        for(int i=0; i<nSheep;i++)
        {
            GameObject newsheep = GameObject.Instantiate(sheepPrefab);
            newsheep.transform.position = new Vector3(Random.Range(sheepXRange.x,sheepXRange.y),0,Random.Range(sheepZRange.x,sheepXRange.y));
        }
    }
}
