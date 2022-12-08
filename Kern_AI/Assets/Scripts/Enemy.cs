using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    [SerializeField]
    private BehaviourTree tree;

    float temp = 7.1f;

    private void Start() {
        foreach(BehaviourNode node in tree.nodes) {
            node.hasStarted = false;
        }
        tree.treeState = NodeState.Running;

        pickedUpItem += OnItemPickup;
    }

    private void Update() {
        tree.Update();
    }

    private void OnItemPickup() {
    }
}
