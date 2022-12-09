using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStateTextNode : ActionNode {

    [SerializeField]
    private string stateText;

    protected override void OnStart() {}

    protected override NodeState Evaluate() {
        tree.blackboard.SetValue<string>("CurrentState", stateText);
        return NodeState.Succes;
    }

    protected override void OnEnd() {}

}
