using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenericObject<T> {

    public Action<T> OnValueChanged;

    public T Data {
        get { return data; }
        set {
            data = value;
            OnValueChanged?.Invoke(data);
        }
    }
    
    [SerializeField]
    private T data;

    public GenericObject(T _value) {
        data = _value;
    }

    public Type GetObjectType() {
        return typeof(T);
    }

}