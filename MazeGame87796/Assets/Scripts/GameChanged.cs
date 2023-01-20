using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChanged : MonoBehaviour
{
    public string Name;

    public GameObject[] MapObjects;
    public Material[] Grass;
    public Material[] Stone;

    public GameObject ToSpawn;

    private void Start()
    {
        Collectable[] collectables = FindObjectsOfType<Collectable>();
        for (int i = 0; i < collectables.Length; i++)
        {
            if (collectables[i].ID == -1)
            {
                collectables[i].EventGameStateChange.Add(this.gameObject);
            }
        }
    }

    public void StateUpdate(int Newstate)
    {
        switch (Name){
            case "Map":
                for (int i = 0; i < MapObjects.Length; i++)
                {
                    MeshRenderer renderer = MapObjects[i].GetComponent<MeshRenderer>();
                    Material[] materials = renderer.materials;
                    materials[0] = Grass[Newstate];
                    if (materials.Length > 1)
                    {
                        materials[1] = Stone[Newstate];
                    }
                    renderer.materials = materials;
                }
                break;
            case "RegretSpawner":
                if (Newstate == 2)
                {
                    Instantiate(ToSpawn, this.gameObject.transform);
                }
                break;
        }
    }
}
