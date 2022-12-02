using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    [SerializeField]
    private BehaviourTree tree;

    private void Start() {
        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
        tree.treeState = NodeState.Running;

        tree.blackboard.SetValue<bool>("Has Weapon", false);

        pickedUpItem += OnItemPickup;
    }

    private void Update() {
        tree.Update();
    }

    private void OnItemPickup() {
        tree.blackboard.SetValue<bool>("Has Weapon", true);
    }
}
