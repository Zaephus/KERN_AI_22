using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertorNode : DecoratorNode {

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        if(GetChildren()[0].Update() == NodeState.Failure) {
            return NodeState.Succes;
        }
        else if(GetChildren()[0].Update() == NodeState.Succes) {
            return NodeState.Failure;
        }
        return NodeState.Running;
    }

    protected override void OnEnd() {}
    
}
