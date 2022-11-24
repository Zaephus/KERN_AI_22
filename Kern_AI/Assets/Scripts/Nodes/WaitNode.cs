using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode {

    [NodeProperty]
    public float duration = 1f;
    private float startTime;

    protected override void OnStart() {
        startTime = Time.time;
    }

    protected override NodeState Evaluate() {
        if(Time.time - startTime >= duration) {
            return NodeState.Succes;
        }
        else {
            return NodeState.Running;
        }
    }

    protected override void OnEnd() {}

}