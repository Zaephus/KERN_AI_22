using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestWithTagNode : ActionNode {

    [NodeProperty(NodePropertyType.Null), SerializeReference]
    public string objectTag;
    [NodeProperty(NodePropertyType.ReadOnly), SerializeReference]
    public GameObject agent;
    [NodeProperty(NodePropertyType.Null), SerializeReference]
    public Transform test;

    private List<GameObject> objectsToFind;

    protected override void OnStart() {
        objectsToFind.Clear();
        objectsToFind.AddRange(GameObject.FindGameObjectsWithTag(objectTag));
    }

    protected override NodeState Evaluate() {

        if(objectsToFind == null || objectsToFind.Count == 0) {
            return NodeState.Failure;
        }

        GameObject obj = objectsToFind[0];
        for(int i = 1; i < objectsToFind.Count; i++) {
            if(Vector3.Distance(agent.transform.position, objectsToFind[i].transform.position) < Vector3.Distance(agent.transform.position, obj.transform.position)) {
                obj = objectsToFind[i];
            }
        }
        
        return NodeState.Succes;

    }

    protected override void OnEnd() {}

}
