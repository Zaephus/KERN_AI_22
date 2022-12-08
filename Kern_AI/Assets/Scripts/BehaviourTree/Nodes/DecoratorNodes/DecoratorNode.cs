using System.Collections;
using System.Collections.Generic;

//Could this be an interface?
public abstract class DecoratorNode : BehaviourNode {

    public BehaviourNode child;

    public override void AddChild(BehaviourNode _child) {
        child = _child;
    }

    public override void RemoveChild(BehaviourNode _child) {
        if(child == _child) {
            child = null;
        }
    }

    public override List<BehaviourNode> GetChildren() {
        if(child == null) {
            return null;
        }
        return new List<BehaviourNode>() { child };
    }

}
