using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    None = 0,
    SawPlayer, //When the ai sees the player
    LostPlayer, //When the ai is out of range of the player
    FoundFriend, //When the ai makes a friend and starts behaving as a boid.
    InPen, //When the ai is in the pen
}

public enum FSMStateID
{
    None = 0,
    Wandering,
    Standing,
    Fleeing,
    Boiding,
}
public class FSM : MonoBehaviour
{
    //Thois is the finite state maching (FSM) class
    // Start is called before the first frame update
    

    protected List<FSMState> fsmStates;
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID()
    {
        return currentStateID;
    }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public void AddFSMState(FSMState state)
    {
        Debug.Log(fsmStates);
        if (fsmStates == null)
        {
            fsmStates = new List<FSMState>();
        }
        fsmStates.Add(state);
    }


    public void PerformTransition(Transition t)
    {
        FSMStateID id = currentState.GetOutputState(t);
        if (id == FSMStateID.None)
        {
            Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
                           " for transition " + t.ToString());
            return;
        }
        currentStateID = id;
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == currentStateID)
            {
                Debug.Log(state.ID);
                Debug.Log(currentStateID);
                currentState.BeforeExit(); //perform the BeforeExit() method of the original state
                state.BeforeEnter(); //Perform the BeforeEnter() method of the new state
                currentState = state;
                break;
            }
        }
    }

    public void SetInitialState()
    {
        currentState = fsmStates[0];
    }
}
