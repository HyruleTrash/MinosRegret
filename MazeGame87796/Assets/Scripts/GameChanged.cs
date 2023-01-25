using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChanged : MonoBehaviour
{
    public string Name;

    public GameObject[] MapObjects;
    public Material[] Grass;
    public Material[] Stone;
    public Material[] Leaves;

    public float[] Lowspeeds;
    public float[] Highspeeds;

    public int DissapearAt = 2;

    public GameObject ToSpawn;
    public GameObject player;

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
        switch (Name)
        {
            case "RegretSpawner":
                ToSpawn.GetComponent<Regret>().player = player;
                break;
        }
    }

    public void StateUpdate(int Newstate)
    {
        switch (Name){
            case "Bushes":
                foreach (Transform Child in transform)
                {
                    MeshRenderer renderer = Child.GetComponent<MeshRenderer>();
                    Material[] materials = renderer.materials;
                    if (Newstate < Leaves.Length)
                    {
                        materials[0] = Leaves[Newstate];
                    }
                    renderer.materials = materials;
                }
                break;
            case "Map":
                for (int i = 0; i < MapObjects.Length; i++)
                {
                    MeshRenderer renderer = MapObjects[i].GetComponent<MeshRenderer>();
                    Material[] materials = renderer.materials;
                    if (Newstate < Grass.Length)
                    {
                        materials[0] = Grass[Newstate];
                    }
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
            case "Regret":
                Regret regret = gameObject.GetComponent<Regret>();
                regret.Highspeed = Highspeeds[Newstate];
                Debug.Log(Lowspeeds[Newstate]);
                regret.lowspeed = Lowspeeds[Newstate];
                regret.UpdateCurrSpeed();
                break;
            case "Bull":
                if (Newstate == 2)
                {
                    GameObject.Destroy(this.gameObject);
                }
                break;
        }
    }
}
