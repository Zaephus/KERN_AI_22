using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState {
    Running,
    Succes,
    Failure
}

public abstract class BehaviourNode : ScriptableObject {

    [SerializeField]
    private NodeState state = NodeState.Running;

    [SerializeField]
    private bool hasStarted;

    protected abstract void OnStart();
    protected abstract NodeState Evaluate();
    protected abstract void OnEnd();

    public NodeState Update() {

        if(!hasStarted) {
            OnStart();
            hasStarted = true;
        }
        
        state = Evaluate();

        if(state != NodeState.Failure && state != NodeState.Succes) {
            return state;
        }

        OnEnd();
        hasStarted = false;
        return state;
        
    }

}
