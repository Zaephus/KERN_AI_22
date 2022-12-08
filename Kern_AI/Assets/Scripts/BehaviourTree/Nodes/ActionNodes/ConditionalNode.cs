using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalNode : ActionNode {

    public string conditionPath;

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        if(tree.blackboard.GetValue<bool>(conditionPath)) {
            return NodeState.Succes;
        }
        else {
            return NodeState.Failure;
        }
    }

    protected override void OnEnd() {}
}