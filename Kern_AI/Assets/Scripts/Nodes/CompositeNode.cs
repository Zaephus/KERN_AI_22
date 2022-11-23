using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Could this be an interface?
public abstract class CompositeNode : BehaviourNode {

    public List<BehaviourNode> children = new List<BehaviourNode>();

    public override void AddChild(BehaviourNode _child) {
        children.Add(_child);
    }

    public override void RemoveChild(BehaviourNode _child) {
        children.Remove(_child);
    }

    public override List<BehaviourNode> GetChildren() {
        return children;
    }
    
}