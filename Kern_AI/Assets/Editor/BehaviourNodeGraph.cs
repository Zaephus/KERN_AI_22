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
    private List<DataObject> behaviourDataObjects = new List<DataObject>();
    //private List<DataObject> blackboardDataObjects = new List<DataObject>();
    private List<VisualElement> containers = new List<VisualElement>();
    private List<VisualElement> fields = new List<VisualElement>();

    private SerializedObject nodeObj;
    
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

        nodeObj = new SerializedObject(node);

        foreach(KeyValuePair<DataObject, NodePropertyType> kvp in GetNodeProperties()) {

            VisualElement propertyContainer = new VisualElement();
            extensionContainer.Add(propertyContainer);
            containers.Add(propertyContainer);

            DataObject obj = kvp.Key;
            behaviourDataObjects.Add(obj);

            //blackboardDataObjects.Add(null);

            VisualElement field = obj.CreateBindableField(obj.name, obj.name, nodeObj);
            //VisualElement field = obj.CreateField(obj.name);
            
            if(field == null) {
                continue;
            }

            field.SetEnabled(true);
            fields.Add(field);
            propertyContainer.Add(field);

            PropertyPort port = obj.CreatePropertyPort(Direction.Input);
            inputContainer.Add(port);

            port.OnConnect += OnPropertyPortConnect;
            port.OnDisconnect += OnPropertyPortDisconnect;
            
            switch(kvp.Value) {
                case NodePropertyType.Null:
                    propertyContainer.style.paddingLeft = new StyleLength(20f);
                    propertyContainer.Add(port);
                    break;

                case NodePropertyType.ReadOnly:
                    propertyContainer.style.paddingLeft = new StyleLength(20f);
                    propertyContainer.Add(port);
                    field.SetEnabled(false);
                    field.name = "ReadOnly";
                    break;

                case NodePropertyType.NonSerializable:
                    inputContainer.Remove(port);
                    break;

            }

            Rect fieldRect = new Rect(propertyContainer.contentRect.x, propertyContainer.contentRect.y-5, 50, 30);
            
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

    private Dictionary<DataObject, NodePropertyType> GetNodeProperties() {

        Dictionary<DataObject, NodePropertyType> fieldInfos = new Dictionary<DataObject, NodePropertyType>();

        FieldInfo[] infos = node.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        foreach(FieldInfo info in infos) {
            NodeProperty nodeProperty = info.GetCustomAttribute(typeof(NodeProperty)) as NodeProperty;
            if(nodeProperty != null) {
                DataObject obj = new DataObject(info.GetValue(node));
                obj.name = info.Name;
                fieldInfos.Add(obj, nodeProperty.propertyType);
            }
        }

        return fieldInfos;

    }

    private void OnPropertyPortConnect(PropertyPort _port, Edge _edge) {

        if(!propertyPorts.Contains(_port)) {
            Debug.LogWarning("Property port was not found");
            return;
        }

        int index = propertyPorts.IndexOf(_port);

        BlackboardNodeGraph dataGraph = _edge.output.node as BlackboardNodeGraph;
        dataGraph.node.field.Bind(node, behaviourDataObjects[index].name);
        dataGraph.node.field.Update();

        fields[index].SetEnabled(false);

    }

    private void OnPropertyPortDisconnect(PropertyPort _port, Edge _edge) {

        int index = propertyPorts.IndexOf(_port);

        BlackboardNodeGraph dataGraph = _edge.output.node as BlackboardNodeGraph;
        dataGraph.node.field.Unbind(node, behaviourDataObjects[index].name);

        if(fields[index].name != "ReadOnly") {
            fields[index].SetEnabled(true);
        }

    }

    // private void UpdateField(DataObject _obj) {

    //     int index = blackboardDataObjects.IndexOf(_obj);

    //     behaviourDataObjects[index].Data = blackboardDataObjects[index].Data;

    //     containers[index].Remove(fields[index]);
    //     fields[index] = behaviourDataObjects[index].CreateBindableField(behaviourDataObjects[index].name, behaviourDataObjects[index].name, nodeObj);
    //     containers[index].Add(fields[index]);


    // }

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