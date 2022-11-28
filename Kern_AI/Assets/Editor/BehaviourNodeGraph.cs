using System;
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

    public List<PropertyPort> propertyPorts = new List<PropertyPort>();

    public List<SerializedProperty> propertyFields = new List<SerializedProperty>();
    public Dictionary<SerializedProperty, SerializableObject> propertyObjectsDict = new Dictionary<SerializedProperty, SerializableObject>();

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

        foreach(string s in GetNodeProperties().Keys) {
        
            SerializedProperty nodeProperty = nodeObj.FindProperty(s);

            PropertyField propertyField = new PropertyField(nodeProperty);
            propertyField.Bind(nodeObj);

            propertyFields.Add(nodeProperty);
            propertyObjectsDict.Add(nodeProperty, null);

            VisualElement propertyContainer = new VisualElement();
            extensionContainer.Add(propertyContainer);

            propertyContainer.Add(propertyField);
            
            propertyField.SetEnabled(true);

            PropertyPort port = CreatePropertyPort(nodeProperty.type);

            Rect fieldRect = new Rect(propertyField.parent.contentRect.x, propertyField.parent.contentRect.y-5, 50, 30);

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
        foreach(FieldInfo info in infos) {
            NodeProperty nodeProperty = info.GetCustomAttribute(typeof(NodeProperty)) as NodeProperty;
            if(nodeProperty != null) {
                properties.Add(info.Name, nodeProperty.propertyType);
            }
        }

        return properties;

    }

    private void OnPropertyPortConnect(PropertyPort _port, Edge _edge) {

        if(!propertyPorts.Contains(_port)) {
            Debug.Log("Property port was not found");
            return;
        }

        Debug.Log("Connecting Ports");

        int portIndex = propertyPorts.IndexOf(_port);

        SerializedProperty propertyField = propertyFields[portIndex];

        BlackboardNodeGraph dataGraph = _edge.output.node as BlackboardNodeGraph;

        propertyObjectsDict[propertyField] = dataGraph.node.nodeObject;
        Debug.Log(propertyField.intValue);
        propertyField.intValue = (int)propertyObjectsDict[propertyField].GetValue(); 

        SerializedObject nodeObj = new SerializedObject(node);   

        propertyField.serializedObject.ApplyModifiedProperties();

        nodeObj.FindProperty(propertyField.propertyPath).intValue = (int)propertyObjectsDict[propertyField].GetValue();
        Debug.Log(propertyField.intValue);

    }

    private void OnPropertyPortDisconnect(PropertyPort _port, Edge _edge) {}

    private void CreateInputPorts() {
        input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(BehaviourNode));
        if(input == null) {
            return;
        }
        input.portName = "";
        inputContainer.Add(input);
    }

    private PropertyPort CreatePropertyPort(object _obj) {

        PropertyPort port;

        switch(_obj) {

            case "int":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(int));
                break;
                
            case "float":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
                break;

            case "long":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(long));
                break;

            case "string":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(string));
                break;

            case "Vector2":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Vector2));
                break;

            case "Vector2Int":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Vector2Int));
                break;

            case "Vector3":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Vector3));
                break;

            case "Vector3Int":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Vector3Int));
                break;

            case "PPtr<$Object>":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(UnityEngine.Object));
                break;

            default:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(object));
                break;
        }
        
        port.portColor = Color.yellow;
        port.portName = "";

        port.OnConnect += OnPropertyPortConnect;
        port.OnDisconnect += OnPropertyPortDisconnect;
        
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