using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    public GameObject SpawnerPrefab;
    public GameObject SpawnerPrefab2;
    public GameObject SpawnerPrefab3;
    public float PowerUpFrequency = 6f;
    public float HatFrequency = 6f;

    Transform[] allChildren;
    public float coinSize = 5f;
    public bool Spawning = false;
    public float spawnFrequency = 7f;
    private float spawnTimer = 0f;

    public string GizmoIcon;

    private void Start()
    {
        allChildren = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            allChildren[i] = this.transform.GetChild(i).transform;
        }
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawning == true)
        { 
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnFrequency)
            {
                Spawn();
                spawnTimer = 0f;
            }
        }
    }
    
    public void Spawn()
    {
        for (int j = 0; j < this.transform.childCount; j++)
        {
            for (int i = 0; i < this.transform.GetChild(j).transform.childCount; i++)
            {
                GameObject.Destroy(this.transform.GetChild(j).transform.GetChild(i).transform.gameObject);
            }
        }

        for (int i = 0; i < allChildren.Length - 1; i++){
            if (this.gameObject.transform.GetChild(0).transform.childCount <= 20)
            {
                float stepSize = 2f;
                while(Vector3.Distance(allChildren[i].position, Vector3.Lerp(allChildren[i].position, allChildren[i + 1].position, (1 / stepSize))) > coinSize)
                {
                    stepSize += 0.1f;
                    if (Vector3.Distance(allChildren[i].position, Vector3.Lerp(allChildren[i].position, allChildren[i + 1].position, (1 / stepSize))) < coinSize)
                    {
                        break;
                    }
                }

                for (int j = 0; j < stepSize; j++)
                {
                    Vector3 spawnlocation = Vector3.Lerp(allChildren[i].position, allChildren[i + 1].position, ((1 /stepSize)*j));

                    float tempRand = Random.Range(0, 100);
                    GameObject spawnInstance;
                    if (tempRand < PowerUpFrequency)
                    {
                        spawnInstance = Instantiate(SpawnerPrefab2);
                    }
                    else if (tempRand < HatFrequency + PowerUpFrequency && tempRand > PowerUpFrequency)
                    {
                        spawnInstance = Instantiate(SpawnerPrefab3);
                    }
                    else
                    {
                        spawnInstance = Instantiate(SpawnerPrefab);
                    }
                    spawnInstance.transform.position = spawnlocation;
                    spawnInstance.transform.parent = this.gameObject.transform.GetChild(i).transform;
                }                
            }
            Vector3 spawnlocation2 = allChildren[allChildren.Length - 1].position;

            float tempRand2 = Random.Range(0, 100);
            GameObject spawnInstance2;
            if (tempRand2 < PowerUpFrequency)
            {
                spawnInstance2 = Instantiate(SpawnerPrefab2);
            }
            else if (tempRand2 < HatFrequency + PowerUpFrequency && tempRand2 > PowerUpFrequency)
            {
                spawnInstance2 = Instantiate(SpawnerPrefab3);
            }
            else
            {
                spawnInstance2 = Instantiate(SpawnerPrefab);
            }
            spawnInstance2.transform.position = spawnlocation2;
            spawnInstance2.transform.parent = this.gameObject.transform.GetChild(this.transform.childCount - 1).transform;
        }
    }

    private void OnDrawGizmos()
    {
        allChildren = new Transform[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            allChildren[i] = this.transform.GetChild(i).transform;
        }

        for (int i = 0; i < allChildren.Length - 1; i++)
        {
            Gizmos.DrawLine(allChildren[i+1].position, allChildren[i].position);
            Gizmos.DrawIcon(allChildren[i].position, GizmoIcon);
        }

        Gizmos.DrawIcon(allChildren[allChildren.Length - 1].position, GizmoIcon);
    }

}
