using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomNode : CompositeNode {

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        if(GetChildren() == null || GetChildren().Count < 1) {
            Debug.LogWarning("ChooseRandomNode Node has no children.");
            return NodeState.Failure;
        }
        int index = Random.Range(0, GetChildren().Count);
        return GetChildren()[index].Update();
    }

    protected override void OnEnd() {}
    
}
