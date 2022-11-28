using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class BlackboardNode : ScriptableObject {

    public Vector2 nodeGraphPosition;

    public string guid;

    public SerializableObject nodeObject;

    public BehaviourNode connectedNode;
    public int connectedPortIndex;

}