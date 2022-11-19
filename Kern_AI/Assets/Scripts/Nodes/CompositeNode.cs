using System.Collections;
using System.Collections.Generic;

//Could this be an interface?
public abstract class CompositeNode : BehaviourNode {

    public List<BehaviourNode> children = new List<BehaviourNode>();
    
}