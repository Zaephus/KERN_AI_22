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

    private VisualElement field;

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

        field = CreateField(_node.field.dataObject.GetValue());

        extensionContainer.Add(field);

        CreatePropertyPort();

        RefreshPorts();
        RefreshExpandedState();
        expanded = true;

    }

    public override void SetPosition(Rect newPos) {
        base.SetPosition(newPos);
        node.nodeGraphPosition.x = newPos.xMin;
        node.nodeGraphPosition.y = newPos.yMin;
    }

    private void UpdateField(SerializableObject _obj) {
        extensionContainer.Remove(field);
        field = CreateField(_obj.GetValue());
        extensionContainer.Add(field);
    }

    private void CreatePropertyPort() {

        switch(node.field.dataObject.GetValue()) {

            case int:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(int));
                break;

            case float:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
                break;

            case long:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(long));
                break;

            case string:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(string)); 
                break;

            case Vector2:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Vector2));
                break;

            case Vector2Int:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Vector2Int));
                break;

            case Vector3:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Vector3));
                break;

            case Vector3Int:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Vector3Int));
                break;

            case UnityEngine.Object:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(UnityEngine.Object));
                break;

            default:
                output = PropertyPort.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(object));
                break;

        }
        
        output.portColor = Color.yellow;
        output.portName = "";

        outputContainer.Add(output);
    }

    private VisualElement CreateField(object _obj) {

        switch(_obj) {

            case int:
                IntegerField intField = new IntegerField();
                intField.value = (int)_obj;
                intField.SetEnabled(false);
                return intField;

            case float:
                FloatField floatField = new FloatField();
                floatField.value = (float)_obj;
                floatField.SetEnabled(false);
                return floatField;

            case long:
                LongField longField = new LongField();
                longField.value = (long)_obj;
                longField.SetEnabled(false);
                return longField;

            case string:
                TextField textField = new TextField();
                textField.value = (string)_obj;
                textField.SetEnabled(false);
                return textField;

            case Vector2:
                Vector2Field vec2Field = new Vector2Field();
                vec2Field.value = (Vector2)_obj;
                vec2Field.SetEnabled(false);
                return vec2Field;

            case Vector2Int:
                Vector2IntField vec2IntField = new Vector2IntField();
                vec2IntField.value = (Vector2Int)_obj;
                vec2IntField.SetEnabled(false);
                return vec2IntField;

            case Vector3:
                Vector3Field vec3Field = new Vector3Field();
                vec3Field.value = (Vector3)_obj;
                vec3Field.SetEnabled(false);
                return vec3Field;

            case Vector3Int:
                Vector3IntField vec3IntField = new Vector3IntField();
                vec3IntField.value = (Vector3Int)_obj;
                vec3IntField.SetEnabled(false);
                return vec3IntField;

            case UnityEngine.Object:
                ObjectField objectField = new ObjectField();
                objectField.value = (UnityEngine.Object)_obj;
                objectField.SetEnabled(false);
                return objectField;

            default:
                return null;


        }
    }
    
}