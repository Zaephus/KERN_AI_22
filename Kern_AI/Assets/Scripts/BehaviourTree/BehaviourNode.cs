using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState {
    Running = 0,
    Succes = 1,
    Failure = 2
}

[System.Serializable]
public abstract class BehaviourNode : ScriptableObject {

    public Vector2 nodeGraphPosition;

    public string guid;

    public bool hasStarted;

    public BehaviourTree tree;

    [SerializeField]
    private NodeState state = NodeState.Running;

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

    public virtual void AddChild(BehaviourNode _child) {}
    public virtual void RemoveChild(BehaviourNode _child) {}
    public virtual List<BehaviourNode> GetChildren() {
        List<BehaviourNode> children = new List<BehaviourNode>();
        return children;
    }

}
