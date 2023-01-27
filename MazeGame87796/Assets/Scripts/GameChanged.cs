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

    public float[] FoggDensities;
    public Color[] FoggColors;

    public float[] Lowspeeds;
    public float[] Highspeeds;
    public float Changespeed = 0.7f;
    public float ChangespeedDensity = 1;

    public int DissapearAt = 2;

    public GameObject ToSpawn;
    public GameObject player;

    public GameObject LeavesParticles;
    public GameObject DustParticles;

    float DensityTowardsFogg;
    Color ColorTowardsFogg;

    private void Start()
    {
        DensityTowardsFogg = RenderSettings.fogDensity;
        ColorTowardsFogg = RenderSettings.fogColor;

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

    private void Update()
    {
        switch (Name)
        {
            case "Map":
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, ColorTowardsFogg, Time.deltaTime * Changespeed);
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, DensityTowardsFogg, Time.deltaTime * ChangespeedDensity);
                RenderSettings.fog = true;
                break;
        }
    }

    public void StateUpdate(int Newstate)
    {
        switch (Name){
            case "Player":
                if (Newstate == 2)
                {
                    LeavesParticles.GetComponent<ParticleSystem>().Stop();
                }
                else if (Newstate == 3)
                {
                    DustParticles.GetComponent<ParticleSystem>().Play();
                }
                break;
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

                    if (FoggColors.Length > Newstate)
                    {
                        ColorTowardsFogg = FoggColors[Newstate];
                        DensityTowardsFogg = FoggDensities[Newstate];
                    }
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
