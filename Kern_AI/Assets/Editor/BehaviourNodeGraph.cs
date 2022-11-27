using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourNodeGraph : NodeGraph {

    public BehaviourNode node;

    public Port input;
    public Port output;

    public List<Port> propertyPorts = new List<Port>();

    public BehaviourNodeGraph(BehaviourNode _node) {

        base.behaviourNodeGraph = this;

        node = _node;
        if(node == null) {
            return;
        }
        base.title = node.name;
        viewDataKey = node.guid;
        
        style.left = node.nodeGraphPosition.x;
        style.top = node.nodeGraphPosition.y;

        SerializedObject nodeObj = new SerializedObject(node);

        int i = -5;

        foreach(string s in GetNodeProperties().Keys) {
        
            SerializedProperty nodeProperty = nodeObj.FindProperty(s);

            PropertyField propertyField = new PropertyField(nodeProperty);
            propertyField.Bind(nodeObj);

            extensionContainer.Add(propertyField);

            propertyField.SetEnabled(true);

            Port port = CreatePropertyPort();

            Rect fieldRect = new Rect(propertyField.parent.contentRect.x, propertyField.parent.contentRect.y + i, 50, 30);

            switch(GetNodeProperties()[s]) {
                case NodePropertyType.Null:
                    propertyField.parent.style.paddingLeft = new StyleLength(20f);
                    propertyField.parent.Add(port);
                    port.SetPosition(fieldRect);
                    propertyPorts.Add(port);
                    break;

                case NodePropertyType.ReadOnly:
                    propertyField.parent.style.paddingLeft = new StyleLength(20f);
                    propertyField.parent.Add(port);
                    port.SetPosition(fieldRect);
                    propertyPorts.Add(port);
                    propertyField.SetEnabled(false);
                    break;

                case NodePropertyType.NonSerializable:
                    inputContainer.Remove(port);
                    break;

            }

            i += 19;

        }

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

    private Dictionary<string, NodePropertyType> GetNodeProperties() {

        Dictionary<string, NodePropertyType> properties = new Dictionary<string, NodePropertyType>();

        FieldInfo[] infos = node.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        foreach(MemberInfo info in infos) {
            NodeProperty nodeProperty = info.GetCustomAttribute(typeof(NodeProperty)) as NodeProperty;
            if(nodeProperty != null) {
                properties.Add(info.Name, nodeProperty.propertyType);
            }
        }

        return properties;

    }

    private void CreateInputPorts() {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(BehaviourNode));
        if(input == null) {
            return;
        }
        input.portName = "";
        inputContainer.Add(input);
    }

    private Port CreatePropertyPort() {
        Port port = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(object));
        port.portColor = Color.yellow;
        port.portName = "";
        
        inputContainer.Add(port);
        return port;
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
