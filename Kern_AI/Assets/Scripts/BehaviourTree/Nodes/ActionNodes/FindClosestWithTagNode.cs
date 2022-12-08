using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestWithTagNode : ActionNode {

    public string objectTag;

    private Enemy agent;
    private List<GameObject> objectsToFind;

    protected override void OnStart() {
        agent = tree.blackboard.GetValue<Enemy>("Agent");
        objectsToFind.Clear();
        objectsToFind.AddRange(GameObject.FindGameObjectsWithTag(objectTag));
    }

    protected override NodeState Evaluate() {

        if(objectsToFind == null || objectsToFind.Count == 0) {
            return NodeState.Failure;
        }

        GameObject obj = null;
        float shortestDistance = float.MaxValue;
        for(int i = 0; i < objectsToFind.Count; i++) {
            if(objectsToFind[i].Equals(agent.gameObject)) {
                continue;
            }
            float dist = Vector3.Distance(agent.transform.position, objectsToFind[i].transform.position);
            if(dist < shortestDistance) {
                shortestDistance = dist;
                obj = objectsToFind[i];
            }
        }

        if(obj == null) {
            return NodeState.Failure;
        }
        tree.blackboard.SetValue<GameObject>("CurrentTarget", obj);
        
        return NodeState.Succes;

    }

    protected override void OnEnd() {}

}
