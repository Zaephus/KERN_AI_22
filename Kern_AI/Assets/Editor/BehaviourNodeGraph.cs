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

public class BehaviourNodeGraph : NodeGraph {

    public BehaviourNode node;

    public Port input;
    public Port output;

    public List<PropertyPort> propertyPorts = new List<PropertyPort>();
    public List<SerializedProperty> properties = new List<SerializedProperty>();
    public List<PropertyField> propertyFields = new List<PropertyField>();
    public List<SerializableObject> propertyObjects = new List<SerializableObject>();
    
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

            properties.Add(nodeProperty);
            propertyFields.Add(propertyField);
            propertyObjects.Add(null);

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
                    break;

                case NodePropertyType.ReadOnly:
                    propertyField.parent.style.paddingLeft = new StyleLength(20f);
                    propertyField.parent.Add(port);
                    propertyField.SetEnabled(false);
                    break;

                case NodePropertyType.NonSerializable:
                    inputContainer.Remove(port);
                    break;

            }

            port.SetPosition(fieldRect);
            propertyPorts.Add(port);

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
            Debug.LogWarning("Property port was not found");
            return;
        }

        int portIndex = propertyPorts.IndexOf(_port);

        SerializedProperty property = properties[portIndex];

        BlackboardNodeGraph dataGraph = _edge.output.node as BlackboardNodeGraph;
        propertyObjects[portIndex] = dataGraph.node.field.dataObject;
        dataGraph.node.field.dataObject.OnValueChanged += UpdateField;

        propertyFields[portIndex].SetEnabled(false);
        
        UpdateField(dataGraph.node.field.dataObject);

    }

    private void OnPropertyPortDisconnect(PropertyPort _port, Edge _edge) {

        int portIndex = propertyPorts.IndexOf(_port);

        SerializedProperty property = properties[portIndex];

        propertyObjects[portIndex].OnValueChanged -= UpdateField;
        propertyObjects[portIndex] = null;

        if(GetNodeProperties()[property.propertyPath] != NodePropertyType.ReadOnly) {
            propertyFields[portIndex].SetEnabled(true);
        }

    }

    private void UpdateField(SerializableObject _obj) {

        int index = propertyObjects.IndexOf(_obj);

        SerializedProperty property = properties[index];

        SerializedObject nodeObj = property.serializedObject;

        switch(_obj.GetValue()) {

            case int:
                property.intValue = (int)_obj.GetValue();
                break;

            case float:
                property.floatValue = (float)_obj.GetValue();
                break;

            case long:
                property.longValue = (long)_obj.GetValue();
                break;

            case bool:
                property.boolValue = (bool)_obj.GetValue();
                break;

            case string:
                property.stringValue = (string)_obj.GetValue();
                break;

            case Vector2:
                property.vector2Value = (Vector2)_obj.GetValue();
                break;

            case Vector2Int:
                property.vector2IntValue = (Vector2Int)_obj.GetValue();
                break;

            case Vector3:
                property.vector3Value = (Vector3)_obj.GetValue();
                break;

            case Vector3Int:
                property.vector3IntValue = (Vector3Int)_obj.GetValue();
                break;

            case UnityEngine.Object:
                property.objectReferenceValue = (UnityEngine.Object)_obj.GetValue();
                break;

        }

        nodeObj.ApplyModifiedProperties();

    }

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

            case "bool":
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
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