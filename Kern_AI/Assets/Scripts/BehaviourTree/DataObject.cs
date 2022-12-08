using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

[System.Serializable]
public class DataObject : ISerializationCallbackReceiver {

    public Action<DataObject> OnValueChanged;

    public string name;

    public object Data {
        get { return data; }
        set {
            data = value;
            OnValueChanged?.Invoke(this);
        }
    }

    private object data;

    private enum ObjectType {
        Null,
        Int,
        Float,
        Long,
        Double,
        Bool,
        String,
        Vector2,
        Vector2Int,
        Vector3,
        Vector3Int,
        Transform,
        GameObject,
        Object
    }

    private ObjectType type = ObjectType.Null;

    private GenericObject<int> intObject = null;
    private GenericObject<float> floatObject = null;
    private GenericObject<long> longObject = null;
    private GenericObject<double> doubleObject = null;
    private GenericObject<bool> boolObject = null;
    private GenericObject<string> stringObject = null;
    private GenericObject<Vector2> vector2Object = null;
    private GenericObject<Vector2Int> vector2IntObject = null;
    private GenericObject<Vector3> vector3Object = null;
    private GenericObject<Vector3Int> vector3IntObject = null;
    private GenericObject<Transform> transformObject = null;
    private GenericObject<GameObject> gameObjectObject = null;
    private GenericObject<UnityEngine.Object> objectObject = null;


    public DataObject(object _value) {
        Data = _value;
    }

    public void OnBeforeSerialize() {

        if(data == null) {
            type = ObjectType.Null;
            return;
        }

        switch(data) {

            case int:
                intObject = new GenericObject<int>((int)data);
                type = ObjectType.Int;
                break;

            case float:
                floatObject = new GenericObject<float>((float)data);
                type = ObjectType.Float;
                break;

            case long:
                longObject = new GenericObject<long>((long)data);
                type = ObjectType.Long;
                break;
            
            case double:
                doubleObject = new GenericObject<double>((double)data);
                type = ObjectType.Double;
                break;

            case bool:
                boolObject = new GenericObject<bool>((bool)data);
                type = ObjectType.Bool;
                break;

            case string:
                stringObject = new GenericObject<string>((string)data);
                type = ObjectType.String;
                break;

            case Vector2:
                vector2Object = new GenericObject<Vector2>((Vector2)data);
                type = ObjectType.Vector2;
                break;
            
            case Vector2Int:
                vector2IntObject = new GenericObject<Vector2Int>((Vector2Int)data);
                type = ObjectType.Vector2Int;
                break;

            case Vector3:
                vector3Object = new GenericObject<Vector3>((Vector3)data);
                type = ObjectType.Vector3;
                break;

            case Vector3Int:
                vector3IntObject = new GenericObject<Vector3Int>((Vector3Int)data);
                type = ObjectType.Vector3Int;
                break;

            case Transform:
                transformObject = new GenericObject<Transform>((Transform)data);
                type = ObjectType.Transform;
                break;

            case GameObject:
                gameObjectObject = new GenericObject<GameObject>((GameObject)data);
                type = ObjectType.GameObject;
                break;

            case UnityEngine.Object:
                objectObject = new GenericObject<UnityEngine.Object>((UnityEngine.Object)data);
                type = ObjectType.Object;
                break;

            default:
                type = ObjectType.Null;
                break;

        }

    }

    public void OnAfterDeserialize() {

        if(type == ObjectType.Null) {
            return;
        }

        switch(type) {

            case ObjectType.Int:
                data = intObject.Data;
                break;

            case ObjectType.Float:
                data = floatObject.Data;
                break;

            case ObjectType.Long:
                data = longObject.Data;
                break;

            case ObjectType.Double:
                data = doubleObject.Data;
                break;

            case ObjectType.Bool:
                data = boolObject.Data;
                break;

            case ObjectType.String:
                data = stringObject.Data;
                break;

            case ObjectType.Vector2:
                data = vector2Object.Data;
                break;

            case ObjectType.Vector2Int:
                data = vector2IntObject.Data;
                break;

            case ObjectType.Vector3:
                data = vector3Object.Data;
                break;

            case ObjectType.Vector3Int:
                data = vector3IntObject.Data;
                break;

            case ObjectType.Transform:
                data = transformObject.Data;
                break;

            case ObjectType.GameObject:
                data = gameObjectObject.Data;
                break;

            case ObjectType.Object:
                data = objectObject.Data;
                break;

            default:
                data = null;
                break;

        }
        
    }

    public VisualElement CreateField(string _name) {

        switch(data) {

            case int:
                IntegerField intField = new IntegerField();
                intField.value = (int)data;
                intField.label = _name;
                intField.SetEnabled(false);
                return intField;

            case float:
                FloatField floatField = new FloatField();
                floatField.value = (float)data;
                floatField.label = _name;
                floatField.SetEnabled(false);
                return floatField;

            case long:
                LongField longField = new LongField();
                longField.value = (long)data;
                longField.label = _name;
                longField.SetEnabled(false);
                return longField;

            case double:
                DoubleField doubleField = new DoubleField();
                doubleField.value = (double)data;
                doubleField.label = _name;
                doubleField.SetEnabled(false);
                return doubleField;

            case bool:
                Toggle toggle = new Toggle();
                toggle.value = (bool)data;
                toggle.label = _name;
                toggle.SetEnabled(false);
                return toggle;

            case string:
                TextField textField = new TextField();
                textField.value = (string)data;
                textField.label = _name;
                textField.SetEnabled(false);
                return textField;

            case Vector2:
                Vector2Field vec2Field = new Vector2Field();
                vec2Field.value = (Vector2)data;
                vec2Field.label = _name;
                vec2Field.SetEnabled(false);
                return vec2Field;

            case Vector2Int:
                Vector2IntField vec2IntField = new Vector2IntField();
                vec2IntField.value = (Vector2Int)data;
                vec2IntField.label = _name;
                vec2IntField.SetEnabled(false);
                return vec2IntField;

            case Vector3:
                Vector3Field vec3Field = new Vector3Field();
                vec3Field.value = (Vector3)data;
                vec3Field.label = _name;
                vec3Field.SetEnabled(false);
                return vec3Field;

            case Vector3Int:
                Vector3IntField vec3IntField = new Vector3IntField();
                vec3IntField.value = (Vector3Int)data;
                vec3IntField.label = _name;
                vec3IntField.SetEnabled(false);
                return vec3IntField;

            case Transform:
                ObjectField transformField = new ObjectField();
                transformField.value = (Transform)data;
                transformField.label = _name;
                transformField.SetEnabled(false);
                return transformField;

            case GameObject:
                ObjectField gameObjectField = new ObjectField();
                gameObjectField.value = (GameObject)data;
                gameObjectField.label = _name;
                gameObjectField.SetEnabled(false);
                return gameObjectField;

            case UnityEngine.Object:
                ObjectField objectField = new ObjectField();
                objectField.value = (UnityEngine.Object)data;
                objectField.label = _name;
                objectField.SetEnabled(false);
                return objectField;

            default:
                return null;

        }

    }

    public VisualElement CreateBindableField(string _name, string _path, SerializedObject _obj) {

        SerializedProperty property = _obj.FindProperty(_name);

        switch(data) {

            case int:
                IntegerField intField = new IntegerField();
                intField.value = (int)data;
                property.intValue = (int)data;
                intField.label = _name;
                intField.bindingPath = _path;
                intField.Bind(_obj);
                intField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return intField;

            case float:
                FloatField floatField = new FloatField();
                floatField.value = (float)data;
                property.floatValue = (float)data;
                floatField.label = _name;
                floatField.bindingPath = _path;
                floatField.Bind(_obj);
                floatField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return floatField;

            case long:
                LongField longField = new LongField();
                longField.value = (long)data;
                property.longValue = (long)data;
                longField.label = _name;
                longField.bindingPath = _path;
                longField.Bind(_obj);
                longField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return longField;

            case double:
                DoubleField doubleField = new DoubleField();
                doubleField.value = (double)data;
                property.doubleValue = (double)data;
                doubleField.label = _name;
                doubleField.bindingPath = _path;
                doubleField.Bind(_obj);
                doubleField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return doubleField;

            case bool:
                Toggle toggle = new Toggle();
                toggle.value = (bool)data;
                property.boolValue = (bool)data;
                toggle.label = _name;
                toggle.bindingPath = _path;
                toggle.Bind(_obj);
                toggle.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return toggle;

            case string:
                TextField textField = new TextField();
                textField.value = (string)data;
                property.stringValue = (string)data;
                textField.label = _name;
                textField.bindingPath = _path;
                textField.Bind(_obj);
                textField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return textField;

            case Vector2:
                Vector2Field vec2Field = new Vector2Field();
                vec2Field.value = (Vector2)data;
                property.vector2Value = (Vector2)data;
                vec2Field.label = _name;
                vec2Field.bindingPath = _path;
                vec2Field.Bind(_obj);
                vec2Field.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return vec2Field;

            case Vector2Int:
                Vector2IntField vec2IntField = new Vector2IntField();
                vec2IntField.value = (Vector2Int)data;
                property.vector2IntValue = (Vector2Int)data;
                vec2IntField.label = _name;
                vec2IntField.bindingPath = _path;
                vec2IntField.Bind(_obj);
                vec2IntField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return vec2IntField;

            case Vector3:
                Vector3Field vec3Field = new Vector3Field();
                vec3Field.value = (Vector3)data;
                property.vector3Value = (Vector3)data;
                vec3Field.label = _name;
                vec3Field.bindingPath = _path;
                vec3Field.Bind(_obj);
                vec3Field.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return vec3Field;

            case Vector3Int:
                Vector3IntField vec3IntField = new Vector3IntField();
                vec3IntField.value = (Vector3Int)data;
                property.vector3IntValue = (Vector3Int)data;
                vec3IntField.label = _name;
                vec3IntField.bindingPath = _path;
                vec3IntField.Bind(_obj);
                vec3IntField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return vec3IntField;

            case Transform:
                ObjectField transformField = new ObjectField();
                transformField.value = (Transform)data;
                property.objectReferenceValue = (Transform)data;
                transformField.label = _name;
                transformField.bindingPath = _path;
                transformField.Bind(_obj);
                transformField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return transformField;

            case GameObject:
                ObjectField gameObjectField = new ObjectField();
                gameObjectField.value = (GameObject)data;
                property.objectReferenceValue = (GameObject)data;
                gameObjectField.label = _name;
                gameObjectField.bindingPath = _path;
                gameObjectField.Bind(_obj);
                gameObjectField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return gameObjectField;

            case UnityEngine.Object:
                ObjectField objectField = new ObjectField();
                objectField.value = (UnityEngine.Object)data;
                property.objectReferenceValue = (UnityEngine.Object)data;
                objectField.label = _name;
                objectField.bindingPath = _path;
                objectField.Bind(_obj);
                objectField.SetEnabled(false);
                _obj.ApplyModifiedProperties();
                return objectField;

            default:
                return null;

        }

    }

    public PropertyPort CreatePropertyPort(Direction _dir) {

        PropertyPort port;

        Direction direction = _dir;

        switch(data) {

            case int:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(int));
                break;

            case float:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(float));
                break;

            case long:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(long));
                break;

            case double:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(double));
                break;

            case bool:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(bool));
                break;

            case string:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(string)); 
                break;

            case Vector2:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(Vector2));
                break;

            case Vector2Int:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(Vector2Int));
                break;

            case Vector3:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(Vector3));
                break;

            case Vector3Int:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(Vector3Int));
                break;

            case Transform:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(Transform));
                break;

            case GameObject:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(GameObject));
                break;

            case UnityEngine.Object:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(UnityEngine.Object));
                break;

            default:
                port = PropertyPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(object));
                break;
        }

        port.portColor = Color.yellow;
        port.portName = "";

        return port;

    }

}