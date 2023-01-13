using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bull : MonoBehaviour
{
    public GameObject Player;
    public GameObject LookAtTarget;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPos = Player.transform.position;
        LookAtTarget.transform.position = new Vector3(PlayerPos.x, this.transform.position.y + 7.45f, PlayerPos.z);
        //agent.SetDestination(PlayerPos);
    }
}
