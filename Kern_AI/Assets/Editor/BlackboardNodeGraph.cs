using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class BlackboardNodeGraph : NodeGraph {

    public BlackboardNode node;

    public PropertyPort output;

    private VisualElement element;

    public BlackboardNodeGraph(BlackboardNode _node) {

        base.blackboardNodeGraph = this;

        node = _node;
        if(node == null) {
            return;
        }

        base.title = _node.name.Replace("Blackboard ", "");;
        viewDataKey = node.guid;

        style.left = node.nodeGraphPosition.x;
        style.top = node.nodeGraphPosition.y;

        node.field.dataObject.OnValueChanged += UpdateField;
        Debug.Log(node.field);

        element = _node.field.dataObject.CreateField("");
        extensionContainer.Add(element);

        output = _node.field.dataObject.CreatePropertyPort(Direction.Output);
        outputContainer.Add(output);

        RefreshPorts();
        RefreshExpandedState();
        expanded = true;

    }

    public override void SetPosition(Rect newPos) {
        base.SetPosition(newPos);
        node.nodeGraphPosition.x = newPos.xMin;
        node.nodeGraphPosition.y = newPos.yMin;
    }

    private void UpdateField(DataObject _obj) {
        extensionContainer.Remove(element);
        element = _obj.CreateField("");
        extensionContainer.Add(element);
    }
    
}