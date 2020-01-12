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
        npc.GetComponent<SheepController>().AvoidShepWalk();
    }

    public override void BeforeEnter()
    {
        //throw new System.NotImplementedException();
    }
    public override void BeforeExit()
    {
        //throw new System.NotImplementedException();
    }
}
