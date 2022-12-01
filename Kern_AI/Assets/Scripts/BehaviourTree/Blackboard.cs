using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Blackboard", menuName = "Behaviour Tree/Blackboard")]
public class Blackboard : ScriptableObject {

    public List<BlackboardNode> nodes = new List<BlackboardNode>();

    public List<BlackboardField> fields = new List<BlackboardField>();

    public T GetValue<T>(string _name) {
        if(fields?.Find(x => x.dataName == _name)) {
            return (T)fields[fields.IndexOf(fields.Find(x => x.dataName == _name))].dataObject.GetValue();
        }
        else {
            return default(T);
        }
    }

    public void SetValue<T>(string _name, T _value) {
        if(fields?.Find(x => x.dataName == _name)) {
            fields[fields.IndexOf(fields.Find(x => x.dataName == _name))].dataObject.SetValue(_value);
        }
        else {
            BlackboardField field = ScriptableObject.CreateInstance<BlackboardField>();
            field.name = "Blackboard Field";
            field.dataName = _name;
            field.dataObject = new SerializableObject();
            field.dataObject.SetValue(_value);

            fields.Add(field);

            AssetDatabase.AddObjectToAsset(field, this);
            AssetDatabase.SaveAssets();
        }
    }

    public void RemoveValue(string _name) {
        if(fields?.Find(x => x.dataName == _name)) {
            BlackboardField field = fields.Find(x => x.dataName == _name);
            fields.Remove(field);
            AssetDatabase.RemoveObjectFromAsset(field);
            AssetDatabase.SaveAssets();
        }
    }
    
}