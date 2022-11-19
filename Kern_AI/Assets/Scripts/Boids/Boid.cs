using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    [SerializeField]
    private Material boidMaterial;

    [SerializeField]
    private float boundsRadius = 0.5f;

    private Vector3 velocity;
    private float separationFactor;
    private float alignmentFactor;
    private float cohesionFactor;
    private float centeringFactor;
    private float turnFactor;
    private float viewDist;
    private float collisionAvoidanceDist;
    private float protectedDist;

    private Flock flock;

    private MeshFilter meshFilter;

    public void Initialize(Flock _flock, Vector3 _vel, float _seperationFactor, float _alignmentFactor, float _cohesionFactor, float _centeringFactor, float _turnFactor, float _viewDist, float _collisionAvoidanceDist, float _protectedDist) {
        flock = _flock;
        velocity = _vel;
        separationFactor = _seperationFactor;
        alignmentFactor = _alignmentFactor;
        cohesionFactor = _cohesionFactor;
        centeringFactor = _centeringFactor;
        turnFactor = _turnFactor;
        viewDist = _viewDist;
        collisionAvoidanceDist = _collisionAvoidanceDist;
        protectedDist = _protectedDist;
        GenerateBody();
    }

    public void OnUpdate() {

        Vector3 close = Vector3.zero;
        Vector3 velAvg = Vector3.zero;
        Vector3 posAvg = Vector3.zero;
        int neighbouringBoids = 0;

        foreach(Boid b in flock.boids) {
            if(b != this) {
                if(Vector3.Distance(transform.position, b.transform.position) <= protectedDist) {
                    close += (transform.position - b.transform.position);
                }
                if(Vector3.Distance(transform.position, b.transform.position) <= viewDist) {
                    velAvg += b.velocity;
                    posAvg += b.transform.position;
                    neighbouringBoids++;
                }
            }
        }

        if(neighbouringBoids > 0) {
            velAvg /= neighbouringBoids;
            posAvg /= neighbouringBoids;
        }

        velocity += close * separationFactor;                           //Rule 1: Separation
        velocity += (velAvg - velocity) * alignmentFactor;              //Rule 2: Alignment
        velocity += (posAvg - transform.position) * cohesionFactor;     //Rule 3: Cohesion
        velocity += (Vector3.zero - velocity) * centeringFactor  ;      //Rule 4: Centralize

        if(IsHeadingForCollisision()) {                                 //Rule 5: Bounds
            Vector3 collisionAvoidDir = ObstacleRays();
            Vector3 steerVector = collisionAvoidDir.normalized * flock.maxSpeed - velocity;
            velocity += steerVector * turnFactor;
        }

        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

    }

    private bool IsHeadingForCollisision() {
        RaycastHit hit; 
        if(Physics.SphereCast(transform.position, boundsRadius, transform.forward, out hit, collisionAvoidanceDist)) {
            return true;
        }
        else {
            return false;
        }
    }

    private Vector3 ObstacleRays() {

        Vector3[] rayDirections = GetRayDirections(100);

        for(int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(transform.position, dir);
            if(!Physics.SphereCast(ray, boundsRadius, collisionAvoidanceDist)) {
                return dir;
            }
        }

        return Vector3.forward;

    }

    private Vector3[] GetRayDirections(int _numDirections) {

        Vector3[] directions = new Vector3[_numDirections];

        float goldenRatio = (1 + Mathf.Sqrt (5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for(int i = 0; i < _numDirections; i++) {
            float t = (float) i / _numDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3(x, y, z);
        }

        return directions;

    }

    private void GenerateBody() {

        Vector3[] vertices = new Vector3[5];

        gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        meshFilter.GetComponent<MeshRenderer>().sharedMaterial = boidMaterial;

        vertices[0] = new Vector3(0, 0, 0.5f);
        vertices[1] = new Vector3(0, -0.25f, -0.5f);
        vertices[2] = new Vector3(-0.25f, 0, -0.5f);
        vertices[3] = new Vector3(0, 0.25f, -0.5f);
        vertices[4] = new Vector3(0.25f, 0, -0.5f);

        int[] triangles = {0,2,1, 0,3,2, 0,4,3, 0,1,4, 1,2,3, 1,3,4};

        meshFilter.sharedMesh.Clear();
        meshFilter.sharedMesh.vertices = vertices;
        meshFilter.sharedMesh.triangles = triangles;
        meshFilter.sharedMesh.RecalculateNormals();

    }

}
