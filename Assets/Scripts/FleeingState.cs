using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingState : FSMState
{
    private float shepDistanceLimit = 70;
    private SheepController sheep;
    public FleeingState(Transform npc)
    {
        stateID = FSMStateID.Fleeing;
        sheep = npc.GetComponent<SheepController>();
        shepDistanceLimit = sheep.shepDistLimit;
    }
    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        if (sheep.inPen == true)
        {
            Debug.Log("switch to standingState");
            sheep.SetTransition(Transition.InPen);
        }
        if (Vector3.Distance(npc.position, player.position) > shepDistanceLimit)
        {
            Debug.Log("Switch to Walking state");
            sheep.SetTransition(Transition.LostPlayer);
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        AvoidShepWalk(player, npc);
    }

    public override void BeforeEnter()
    {
        //throw new System.NotImplementedException();
    }
    public override void BeforeExit()
    {
        //throw new System.NotImplementedException();
    }

    public void AvoidShepWalk(Transform player, Transform npc)
    {
        Vector3 awayFromShep = npc.position - player.position;
        float step = sheep.turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(npc.forward, awayFromShep, step, 0.0f);
        npc.rotation = Quaternion.LookRotation(newDir);
        float runSpeed = (sheep.maxSpeed - sheep.speed) * (sheep.shepDistance / sheep.shepDistLimit) + sheep.speed;
        npc.position += runSpeed * npc.forward * Time.deltaTime;
    }
}
