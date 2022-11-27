using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Blackboard", menuName = "Behaviour Tree/Blackboard")]
public class Blackboard : ScriptableObject {

    public List<BlackboardNode> nodes = new List<BlackboardNode>();

    public Dictionary<string, object> values = new Dictionary<string, object>();

    public T GetValue<T>(string _name) {
        return values.ContainsKey(_name) ? (T)values[_name] : default(T);
    }

    public void SetValue<T>(string _name, T _value) {
        if(values.ContainsKey(_name)) {
            values[_name] = _value;
        }
        else {
            values.Add(_name, _value);
        }
    }

    public void RemoveValue(string _name) {
        if(values.ContainsKey(_name)) {
            values.Remove(_name);
        }
    }
    
}