using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float Timer = 0.0f;
    public float Speed = 2;
    public string kind = "Coin";

    void Start()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        switch (kind)
        {
            case "Coin":
                transform.Rotate(0, 0, Speed * Time.deltaTime);
                break;
            case "Hat":
                transform.Rotate(Speed * Time.deltaTime, 0, Speed * Time.deltaTime);
                break;
            case "Orb":
                transform.Rotate(0, Speed * Time.deltaTime, Speed * Time.deltaTime);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (kind)
        {
            case "Coin":
                switch (other.name)
                {
                    case "BullCoin Variant(Clone)":
                        if (other.transform.GetSiblingIndex() > this.transform.GetSiblingIndex())
                        {
                            Destroy(other.gameObject);
                        }
                        break;
                    case "PowerUp Variant(Clone)":
                        Destroy(this.gameObject);
                        break;
                    case "hat Variant(Clone)":
                        Destroy(this.gameObject);
                        break;
                }
                break;
            case "Hat":
                switch (other.name)
                {
                    case "hat Variant(Clone)":
                        if (other.transform.GetSiblingIndex() > this.transform.GetSiblingIndex())
                        {
                            Destroy(other.gameObject);
                        }
                        break;
                    case "PowerUp Variant(Clone)":
                        Destroy(other.gameObject);
                        break;
                    case "BullCoin Variant(Clone)":
                        Destroy(other.gameObject);
                        break;
                }
                break;
            case "Orb":
                switch (other.name)
                {
                    case "PowerUp Variant(Clone)":
                        if (other.transform.GetSiblingIndex() > this.transform.GetSiblingIndex())
                        {
                            Destroy(other.gameObject);
                        }
                        break;
                    case "hat Variant(Clone)":
                        Destroy(this.gameObject);
                        break;
                    case "BullCoin Variant(Clone)":
                        Destroy(other.gameObject);
                        break;
                }
                break;
        }
    }
}
