using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetNode : ActionNode {

    private GameObject currentTarget;
    private Enemy agent;

    protected override void OnStart() {
        currentTarget = tree.blackboard.GetValue<GameObject>("CurrentTarget");
        agent = tree.blackboard.GetValue<Enemy>("Agent");
    }

    protected override NodeState Evaluate() {
        agent.LookAtTarget(currentTarget.transform.position);
        agent.Attack();
        return NodeState.Succes;

    }

    protected override void OnEnd() {}

}