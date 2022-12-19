using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    public int grade;
    public Color[] colorModifier;
    public GameObject GameVars;

    private void Start()
    {
        updateColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "hatRigidVariant Variant(Clone)")
        {
            if (grade == collision.collider.GetComponent<Hat>().grade && grade < colorModifier.Length - 1)
            {
                Destroy(collision.collider.gameObject);
                GameVars.GetComponent<GameVariables>().HatsMerged++;
                grade++;
                updateColor();
            }
        }
    }

    public void updateColor()
    {
        switch (grade)
        {
            default:
                this.transform.GetChild(0).GetComponent<Renderer>().material.color = colorModifier[grade];
                this.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Metallic", 0f);
                this.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colorModifier[grade];
                this.transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Metallic", 0f);
                this.transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                break;
            case 4:
                this.transform.GetChild(0).GetComponent<Renderer>().material.color = colorModifier[grade];
                this.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Metallic", 1f);
                this.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.555f);
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colorModifier[grade];
                this.transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Metallic", 1f);
                this.transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.555f);
                break;
        }
    }
}
