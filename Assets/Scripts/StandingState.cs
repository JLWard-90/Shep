﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState : FSMState
{
    public StandingState()
    {
        stateID = FSMStateID.Standing;
    }

    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        //Once the sheep is standing it does nothing else
    }

    public override void Act(Transform player, Transform npc)
    {
        // throw new System.NotImplementedException();
        //The sheep does nothing if it is standing
        npc.GetComponent<SheepController>().TurnOffWalkAnimation();
    }
}
