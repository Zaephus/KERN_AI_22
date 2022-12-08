using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinNode : ActionNode {

    private Enemy agent;

    protected override void OnStart() {
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        Debug.Log(agent.name + " has won!!!");
        return NodeState.Succes;

    }

    protected override void OnEnd() {}

}