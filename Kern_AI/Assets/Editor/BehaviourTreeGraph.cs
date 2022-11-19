using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourTreeGraph : GraphView {

    public new class UxmlFactory : UxmlFactory<BehaviourTreeGraph, GraphView.UxmlTraits> {}

    public BehaviourTreeGraph() {

        style.flexGrow = 1;
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

    }

}