using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterNode : DecoratorNode {

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        GetChildren()[0].Update();
        return NodeState.Running;
    }

    protected override void OnEnd() {}
    
}