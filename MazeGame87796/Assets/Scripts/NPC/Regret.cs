using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Regret : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        agent.SetDestination(player.transform.position);
    }
}
