using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupRemove : MonoBehaviour
{
    float waitTime = 0;
    float timeOut = 2f;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("WindPowerUP");
        timeOut = this.gameObject.GetComponent<Player>().PowerupTimeOut;
    }

    // Update is called once per frame
    void Update()
    {
        waitTime += Time.deltaTime;
        if (waitTime > timeOut)
        {
            this.gameObject.GetComponent<Player>().currentSpeedModifiers.Remove(this.gameObject.GetComponent<Player>().speedModifierPowerup);
            this.gameObject.GetComponent<Player>().currentPowerUps--;
            Destroy(this.gameObject.GetComponent<PowerupRemove>());
        }
    }
}
