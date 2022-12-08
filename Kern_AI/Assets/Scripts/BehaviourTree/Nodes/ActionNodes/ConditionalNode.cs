using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : ActionNode {

    [NodeProperty(NodePropertyType.ReadOnly), SerializeReference]
    public bool condition;

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        if(condition) {
            return NodeState.Succes;
        }
        else {
            return NodeState.Failure;
        }
    }

    protected override void OnEnd() {}
}