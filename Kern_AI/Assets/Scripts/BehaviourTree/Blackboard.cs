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
        if(fields.Find(x => x.dataName == _name) != null) {
            BlackboardField field = fields.Find(x => x.dataName == _name) as BlackboardField;
            return (T)field.dataObject.GetValue();
        }
        else {
            return default(T);
        }
    }

    public void SetValue<T>(string _name, T _value) {
        if(fields.Find(x => x.dataName == _name) != null) {
            BlackboardField field = fields.Find(x => x.dataName == _name) as BlackboardField;
            field.dataObject.SetValue(_value);
        }
        else {
            BlackboardField field = ScriptableObject.CreateInstance<BlackboardField>();
            field.name = "Blackboard Field";
            field.dataName = _name;
            field.dataObject = new DataObject(_value);

            fields.Add(field);

            AssetDatabase.AddObjectToAsset(field, this);
            AssetDatabase.SaveAssets();
        }
    }

    public void RemoveValue<T>(string _name) {
        if(fields.Find(x => x.dataName == _name) != null) {
            BlackboardField field = fields.Find(x => x.dataName == _name) as BlackboardField;
            fields.Remove(field);
            AssetDatabase.RemoveObjectFromAsset(field);
            AssetDatabase.SaveAssets();
        }
    }
    
}