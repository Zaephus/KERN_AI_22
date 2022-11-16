using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    [SerializeField]
    private Material boidMaterial;

    private Vector3 velocity;
    private float separationFactor;
    private float alignmentFactor;
    private float cohesionFactor;
    private float turnFactor;
    private float viewDist;
    private float protectedDist;

    private Flock flock;

    private MeshFilter meshFilter;

    public void Initialize(Flock _flock, Vector3 _vel, float _seperationFactor, float _alignmentFactor, float _cohesionFactor, float _viewDist, float _protectedDist) {
        flock = _flock;
        velocity = _vel;
        separationFactor = _seperationFactor;
        alignmentFactor = _alignmentFactor;
        cohesionFactor = _cohesionFactor;
        viewDist = _viewDist;
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

        velocity += close * separationFactor;
        velocity += (velAvg - velocity) * alignmentFactor;
        velocity += (posAvg - transform.position) * cohesionFactor;

        transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

        if(transform.position.x > flock.boundingBox.x) {
            transform.position = new Vector3(-flock.boundingBox.x, transform.position.y, transform.position.z);
        }
        if(transform.position.x < -flock.boundingBox.x) {
            transform.position = new Vector3(flock.boundingBox.x, transform.position.y, transform.position.z);
        }
        if(transform.position.y > flock.boundingBox.y) {
            transform.position = new Vector3(transform.position.x, -flock.boundingBox.y, transform.position.z);
        }
        if(transform.position.y < -flock.boundingBox.y) {
            transform.position = new Vector3(transform.position.x, flock.boundingBox.y, transform.position.z);
        }
        if(transform.position.z > flock.boundingBox.z) {
            transform.position = new Vector3(transform.position.x, transform.position.y, -flock.boundingBox.z);
        }
        if(transform.position.z < -flock.boundingBox.z) {
            transform.position = new Vector3(transform.position.x, transform.position.y, flock.boundingBox.z);
        }

        transform.position += velocity * Time.deltaTime;

    }

    //Move
    //Check for all other boids
    //Use raycasts to choose a path to avoid walls

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
