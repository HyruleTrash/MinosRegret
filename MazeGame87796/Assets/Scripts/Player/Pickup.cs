using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public List<GameObject> TouchingCollectableObjects;
    public float throwForceMax = 30;
    public float throwForceMin = 5;
    public float throwForceStepSize = 1.5f;
    float throwCharge = 0;
    bool charging = false;

    void Start()
    {
        TouchingCollectableObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (TouchingCollectableObjects.Count > 0)
            {
                TouchingCollectableObjects[0].GetComponent<Collectable>().activate();
            }
            else
            {
                this.gameObject.transform.parent.gameObject.transform.parent.GetComponent<Player>().Use();
            }
        }
        if (Input.GetKeyDown("q"))
        {
            charging = true;
        }
        if (charging == true)
        {
            if (throwCharge == 0)
            {
                throwCharge = throwForceMin;
            }
            throwCharge += throwForceStepSize * Time.deltaTime;
            if (throwCharge > throwForceMax)
            {
                throwCharge = throwForceMax;
            }
        }
        if (Input.GetKeyUp("q"))
        {
            //this.gameObject.transform.parent.gameObject.transform.parent.GetComponent<Player>().Drop(throwCharge);
            throwCharge = 0;
            charging = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!TouchingCollectableObjects.Contains(other.gameObject) && other.gameObject.tag == "Collectable")
            TouchingCollectableObjects.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (TouchingCollectableObjects.Contains(other.gameObject))
            TouchingCollectableObjects.Remove(other.gameObject);
    }
}
