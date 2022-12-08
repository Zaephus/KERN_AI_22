using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourNodeGraph : Node {

    public BehaviourNode node;

    public Port input;
    public Port output;
    
    public BehaviourNodeGraph(BehaviourNode _node) {

        node = _node;
        if(node == null) {
            return;
        }
        base.title = node.name;
        viewDataKey = node.guid;
        
        style.left = node.nodeGraphPosition.x;
        style.top = node.nodeGraphPosition.y;

        CreateInputPorts();
        CreateOutputPorts();
        
        RefreshPorts();
        RefreshExpandedState();
        expanded = true;
        
    }

    public override void SetPosition(Rect newPos) {
        base.SetPosition(newPos);
        node.nodeGraphPosition.x = newPos.xMin;
        node.nodeGraphPosition.y = newPos.yMin;
    }

    private void CreateInputPorts() {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(BehaviourNode));
        if(input == null) {
            return;
        }
        input.portName = "";
        inputContainer.Add(input);
    }

    private void CreateOutputPorts() {

        switch(node) {
            case ActionNode:
                break;
            
            case CompositeNode:
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(BehaviourNode));
                break;

            case DecoratorNode:
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(BehaviourNode));
                break;

        }

        if(output == null) {
            return;
        }

        output.portName = "";
        outputContainer.Add(output);

    }
        
}