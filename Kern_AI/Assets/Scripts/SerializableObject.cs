using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableObject : ISerializationCallbackReceiver {

    public Action<object> OnValueChanged;

    private object value;

    [SerializeField, HideInInspector]
    private string serializedValue;

    public void SetValue(object _value) {
        if(value != _value) {
            value = _value;
            OnValueChanged?.Invoke(value);
        }
    }

    public object GetValue() {
        return value;
    }

    public void OnBeforeSerialize() {

        if(value == null) {
            serializedValue = "n";
            return;
        }

        var type = value.GetType();

        switch(value) {

            case int:
                serializedValue = "i" + value.ToString();
                break;

            case float:
                serializedValue = "f" + value.ToString();
                break;

            case string:
                serializedValue = "s" + value;
                break;

            case Vector2:
                Vector2 vec2 = (Vector2)value;
                serializedValue = "w" + vec2.x + "|" + vec2.y;
                break;

            case Vector3:
                Vector3 vec3 = (Vector3)value;
                serializedValue = "v" + vec3.x + "|" + vec3.y + "|" + vec3.z;
                break;

            case bool:
                bool b = (bool) value;
                serializedValue = "b" + b;
                break;

        }

    }

    public void OnAfterDeserialize() {

        if(serializedValue.Length == 0) {
            return;
        }

        char type = serializedValue[0];

        switch(type) {

            case 'n':
                value = null;
                break;

            case 'i':
                value = int.Parse(serializedValue.Substring(1));
                break;

            case 'f':
                value = float.Parse(serializedValue.Substring(1));
                break;

            case 's':
                value = serializedValue.Substring(1);
                break;

            case 'w':
                string[] vec2 = serializedValue.Substring(1).Split('|');
                value = new Vector2(float.Parse(vec2[0]), float.Parse(vec2[1]));
                break;

            case 'v':
                string[] vec3 = serializedValue.Substring(1).Split('|');
                value = new Vector3(float.Parse(vec3[0]), float.Parse(vec3[1]), float.Parse(vec3[2]));
                break;

            case 'b':
                value = bool.Parse(serializedValue.Substring(1));
                break;

        }
        
    }

}
