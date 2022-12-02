using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestWithTagNode : ActionNode {

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        return NodeState.Running;
    }

    protected override void OnEnd() {}

}
