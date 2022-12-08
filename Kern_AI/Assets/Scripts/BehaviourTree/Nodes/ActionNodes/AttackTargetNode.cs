using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetNode : ActionNode {

    private Enemy agent;

    protected override void OnStart() {
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {

        agent.Attack();
        return NodeState.Succes;

    }

    protected override void OnEnd() {}

}