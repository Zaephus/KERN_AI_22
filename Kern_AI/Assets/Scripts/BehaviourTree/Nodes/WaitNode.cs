using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode {

    [NodeProperty(NodePropertyType.Null)]
    public float duration = 1f;
    [NodeProperty(NodePropertyType.Null)]
    public bool damn = true;
    [NodeProperty(NodePropertyType.Null)]
    public int hoi = 2;
    [NodeProperty(NodePropertyType.ReadOnly)]
    public int readOnly = 8;
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
