using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class FSMState
{
    //The abstract class that all of the FSM states inherit from
    //FSMState also manages the transitions from one state to another

    protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
    protected FSMStateID stateID;
    public FSMStateID ID {get { return stateID; }}

    public void AddTransition(Transition trans, FSMStateID id)
    {
        if (trans == Transition.None)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }

        if (id == FSMStateID.None)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }

        map.Add(trans, id); //Add the transition to the dictionary
    }

    public void DeleteTransition(Transition trans)
    {
        //Not yet implemented
    }

    public FSMStateID GetOutputState(Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return FSMStateID.None;
    }

    public abstract void Reason(Transform player, Transform npc);

    public abstract void Act(Transform player, Transform npc);

    public abstract void BeforeEnter();

    public abstract void BeforeExit();
}
