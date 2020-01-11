using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : FSMState
{
    private float shepDistanceLimit = 50;
    private SheepController sheep;
 
    public WanderState(Transform npc)
    {
        sheep = npc.GetComponent<SheepController>();
        shepDistanceLimit = sheep.shepDistLimit;
        sheep.InitRandomWalk();
        stateID = FSMStateID.Wandering;
    }

    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        //Check the distance between the player and this npc
        if (sheep.inPen == true)
        {
            Debug.Log("switch to standingState");
            sheep.SetTransition(Transition.InPen);
        }
        if (Vector3.Distance(npc.position, player.position) < shepDistanceLimit)
        {
            float noticeNum = Random.Range(0, 2f);
            if (noticeNum > 1) //50% chance of the sheep noticing the player
            {
                Debug.Log("Switch to Fleeing state");
                sheep.SetTransition(Transition.SawPlayer);
            }
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        //The behaviour associated with this state:
        sheep.SimpleSheepMovement();
    }

    public override void BeforeEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void BeforeExit()
    {
        throw new System.NotImplementedException();
    }
}
