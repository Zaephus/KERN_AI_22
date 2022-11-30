using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class BlackboardField : ScriptableObject {

    public string dataName;
    public SerializableObject dataObject;

    public Action<string, BlackboardField> OnClick;

    private VisualElement field;

    private GroupBox element;

    public VisualElement CreateBlackboardElement() {

        element = new GroupBox();

        Label label = new Label(dataName);
        element.Add(label);

        field = CreateField(dataObject.GetValue());

        dataObject.OnValueChanged += UpdateField;

        element.Add(field);
        element.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

        return element;

    }

    private void OnMouseDown(MouseDownEvent _evt) {
        OnClick?.Invoke(dataName, this);
    }

    private void UpdateField(SerializableObject _obj) {
        element.Remove(field);
        field = CreateField(_obj.GetValue());
        element.Add(field);
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