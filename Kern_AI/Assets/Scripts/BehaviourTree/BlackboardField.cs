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

    [SerializeField]
    public List<BehaviourNode> nodes = new List<BehaviourNode>();
    [SerializeField]
    public List<string> paths = new List<string>();

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

    public void Update() {
        Debug.Log(nodes.Count);
        for(int i = 0; i < nodes.Count; i++) {
            SerializedObject obj = new SerializedObject(nodes[i]);
            SerializedProperty property = obj.FindProperty(paths[i]);
            dataObject.SetPropertyValue(property, obj);
        }
    }

    public void Bind(BehaviourNode _obj, string _path) {
        if(!nodes.Contains(_obj)) {
            nodes.Add(_obj);
            paths.Add(_path);
            //EditorUtility.SetDirty(this);
        }
    }

    public void Unbind(BehaviourNode _obj, string _path) {
        if(nodes.Contains(_obj)) {
            nodes.Remove(_obj);
            paths.Remove(_path);
        }
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