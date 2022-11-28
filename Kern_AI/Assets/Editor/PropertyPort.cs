using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

public class PropertyPort : Port {

    private class DefaultEdgeConnectorListener : IEdgeConnectorListener {

        private GraphViewChange graphViewChange;
        private List<Edge> edgesToCreate;
        private List<GraphElement> edgesToDelete;

        public DefaultEdgeConnectorListener() {
            this.edgesToCreate = new List<Edge>();
            this.edgesToDelete = new List<GraphElement>();
            this.graphViewChange.edgesToCreate = this.edgesToCreate;
        }

        public void OnDropOutsidePort(Edge _edge, Vector2 _position) {}

        public void OnDrop(GraphView _graphView, Edge _edge) {

            this.edgesToCreate.Clear();
            this.edgesToCreate.Add(_edge);
            this.edgesToDelete.Clear();
            
            if(_edge.input.capacity == Port.Capacity.Single) {
                foreach(Edge connection in _edge.input.connections) {
                    if(connection != _edge) {
                        this.edgesToDelete.Add((GraphElement)connection);
                    }
                }
            }
            if(_edge.output.capacity == Port.Capacity.Single) {
                foreach(Edge connection in _edge.output.connections) {
                    if(connection != _edge) {
                        this.edgesToDelete.Add((GraphElement)connection);
                    }
                }
            }

            if(this.edgesToDelete.Count > 0) {
                _graphView.DeleteElements((IEnumerable<GraphElement>) this.edgesToDelete);
            }

            List<Edge> tempEdgesToCreate = edgesToCreate;
            if(_graphView.graphViewChanged != null) {
                tempEdgesToCreate = _graphView.graphViewChanged(this.graphViewChange).edgesToCreate;
            }

            foreach(Edge e in tempEdgesToCreate) {
                _graphView.AddElement((GraphElement) e);
                _edge.input.Connect(e);
                _edge.output.Connect(e);
            }

        }

    }

    public Action<PropertyPort, Edge> OnConnect;
    public Action<PropertyPort, Edge> OnDisconnect;

    protected PropertyPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type) {}

    public override void Connect(Edge _edge) {
        base.Connect(_edge);
        OnConnect?.Invoke(this, _edge);
    }

    public override void Disconnect(Edge _edge) {
        base.Disconnect(_edge);
        OnDisconnect?.Invoke(this, _edge);
    }

    public override void DisconnectAll() {
        base.DisconnectAll();
        OnDisconnect?.Invoke(this, null);
    }

    public new static PropertyPort Create<TEdge>(Orientation _orientation, Direction _direction, Port.Capacity _capacity, System.Type _type) where TEdge : Edge, new() {

        var listener = new PropertyPort.DefaultEdgeConnectorListener();

        var element = new PropertyPort(_orientation, _direction, _capacity, _type) {
            m_EdgeConnector = (EdgeConnector) new EdgeConnector<TEdge>((IEdgeConnectorListener) listener)
        };

        element.AddManipulator((IManipulator)element.m_EdgeConnector);
        return element;

    }

}