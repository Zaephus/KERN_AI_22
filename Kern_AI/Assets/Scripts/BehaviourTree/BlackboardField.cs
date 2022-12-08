using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class BlackboardField : ScriptableObject {

    public string dataName;
    public DataObject dataObject;

    public Action<string, BlackboardField> OnClick;

    private VisualElement element;

    private GroupBox box;

    public VisualElement CreateBlackboardElement() {

        box = new GroupBox();

        Label label = new Label(dataName);
        box.Add(label);

        element = dataObject.CreateField("");

        dataObject.OnValueChanged += UpdateField;

        box.Add(element);
        box.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

        return box;

    }

    private void OnMouseDown(MouseDownEvent _evt) {
        if(_evt.button == 0) {
            OnClick?.Invoke(dataName, this);
        }
    }

    private void UpdateField(DataObject _obj) {
        box.Remove(element);
        element = _obj.CreateField("");
        box.Add(element);
    }
    
}