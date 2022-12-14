using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode {

    public string message;

    protected override void OnStart() {
        Debug.Log("OnStart: " + message);
    }

    protected override NodeState Evaluate() {
        Debug.Log("OnUpdate: " + message);
        return NodeState.Succes;
    }

    protected override void OnEnd() {
        Debug.Log("OnEnd: " + message);
    }
    
}
