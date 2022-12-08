using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    [SerializeField]
    private BehaviourTree tree;

    float temp = 0.1f;

    private void Start() {
        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
        tree.treeState = NodeState.Running;

        tree.blackboard.fields.Clear();

        tree.blackboard.SetValue<bool>("Has Weapon", true);
        tree.blackboard.SetValue<float>("Test", 0.7f);
        tree.blackboard.SetValue<GameObject>("Current Target", gameObject);
        tree.blackboard.SetValue<Transform>("Actor Transform", transform);

        pickedUpItem += OnItemPickup;
    }

    private void Update() {
        temp += 0.01f;
        tree.blackboard.SetValue<float>("Test", temp);
        tree.Update();
    }

    private void OnItemPickup() {
        tree.blackboard.SetValue<bool>("Has Weapon", true);
    }
}
