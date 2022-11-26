using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Blackboard : ScriptableObject {

    public Dictionary<string, object> values = new Dictionary<string, object>();
    [SerializeField]
    public List<object> objects = new List<object>();

    public T GetValue<T>(string _name) {
        return values.ContainsKey(_name) ? (T)values[_name] : default(T);
    }

    public void SetValue<T>(string _name, T _value) {
        if(values.ContainsKey(_name)) {
            objects[objects.IndexOf(values[_name])] = _value;
            values[_name] = _value;
        }
        else {
            objects.Add(_value);
            values.Add(_name, _value);
        }
    }

    public void RemoveValue(string _name) {
        if(values.ContainsKey(_name)) {
            objects.Remove(values[_name]);
            values.Remove(_name);
        }
    }
    
}