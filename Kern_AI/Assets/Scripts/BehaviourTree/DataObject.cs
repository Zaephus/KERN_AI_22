using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataObject : ISerializationCallbackReceiver {

    public Action<DataObject> OnValueChanged;

    private object value;

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
        SetValue(_value);
    }

    public void SetValue(object _value) {
        if(value != _value) {
            value = _value;
            OnValueChanged?.Invoke(this);
        }
    }

    public object GetValue() {
        return value;
    }

    public void OnBeforeSerialize() {

        if(value == null) {
            type = ObjectType.Null;
            return;
        }

        switch(value) {

            case int:
                intObject = new GenericObject<int>((int)value);
                type = ObjectType.Int;
                break;

            case float:
                floatObject = new GenericObject<float>((float)value);
                type = ObjectType.Float;
                break;

            case long:
                longObject = new GenericObject<long>((long)value);
                type = ObjectType.Long;
                break;
            
            case double:
                doubleObject = new GenericObject<double>((double)value);
                type = ObjectType.Double;
                break;

            case bool:
                boolObject = new GenericObject<bool>((bool)value);
                type = ObjectType.Bool;
                break;

            case string:
                stringObject = new GenericObject<string>((string)value);
                type = ObjectType.String;
                break;

            case Vector2:
                vector2Object = new GenericObject<Vector2>((Vector2)value);
                type = ObjectType.Vector2;
                break;
            
            case Vector2Int:
                vector2IntObject = new GenericObject<Vector2Int>((Vector2Int)value);
                type = ObjectType.Vector2Int;
                break;

            case Vector3:
                vector3Object = new GenericObject<Vector3>((Vector3)value);
                type = ObjectType.Vector3;
                break;

            case Vector3Int:
                vector3IntObject = new GenericObject<Vector3Int>((Vector3Int)value);
                type = ObjectType.Vector3Int;
                break;

            case Transform:
                transformObject = new GenericObject<Transform>((Transform)value);
                type = ObjectType.Transform;
                break;

            case GameObject:
                gameObjectObject = new GenericObject<GameObject>((GameObject)value);
                type = ObjectType.GameObject;
                break;

            case UnityEngine.Object:
                objectObject = new GenericObject<UnityEngine.Object>((UnityEngine.Object)value);
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
                value = intObject.Data;
                break;

            case ObjectType.Float:
                value = floatObject.Data;
                break;

            case ObjectType.Long:
                value = longObject.Data;
                break;

            case ObjectType.Double:
                value = doubleObject.Data;
                break;

            case ObjectType.Bool:
                value = boolObject.Data;
                break;

            case ObjectType.String:
                value = stringObject.Data;
                break;

            case ObjectType.Vector2:
                value = vector2Object.Data;
                break;

            case ObjectType.Vector2Int:
                value = vector2IntObject.Data;
                break;

            case ObjectType.Vector3:
                value = vector3Object.Data;
                break;

            case ObjectType.Vector3Int:
                value = vector3IntObject.Data;
                break;

            case ObjectType.Transform:
                value = transformObject.Data;
                break;

            case ObjectType.GameObject:
                value = gameObjectObject.Data;
                break;

            case ObjectType.Object:
                value = objectObject.Data;
                break;

            default:
                value = null;
                break;

        }
        
    }

}
