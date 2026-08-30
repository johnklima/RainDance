using Alteruna.Multiplayer;
using Alteruna.Multiplayer.Core;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : CommunicationBridge
{

    NavMeshAgent agent;
    public Transform target;

    public override void Possessed(bool isMe, User user)
    {

        enabled = false;
        // disables this script for remote players
        enabled = isMe;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
