using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public List<Boid> boids = new List<Boid>();

    public float minSpeed;
    public float maxSpeed;

    public float separationFactor;
    public float alignmentFactor;
    public float cohesionFactor;
    public float centeringFactor;
    public float turnFactor;
    public float viewDist;
    public float collisionAvoidanceDist;
    public float protectedDist;

    public Vector3 boundingBox = new Vector3();

    [SerializeField]
    private int boidAmount;
    [SerializeField]
    private GameObject boidPrefab;

    private void OnValidate() {
        transform.localScale = boundingBox + new Vector3(0.25f, 0.25f, 0.25f);
    }

    private void Start() {

        for(int i = 0; i < boidAmount; i++) {

            Vector3 boidPos = new Vector3(Random.Range(-boundingBox.x + 1, boundingBox.x - 1),
                                          Random.Range(-boundingBox.y + 1, boundingBox.y - 1),
                                          Random.Range(-boundingBox.z + 1, boundingBox.z - 1));

            Quaternion boidRot = Random.rotation;

            Vector3 boidVel = new Vector3(Random.Range(minSpeed, maxSpeed),
                                          Random.Range(minSpeed, maxSpeed),
                                          Random.Range(minSpeed, maxSpeed));
            
            GameObject boid = Instantiate(boidPrefab, boidPos, boidRot);
            boid.GetComponent<Boid>().Initialize(this, boidVel, separationFactor, alignmentFactor, cohesionFactor, centeringFactor, turnFactor, viewDist, collisionAvoidanceDist, protectedDist);
            boids.Add(boid.GetComponent<Boid>());

        }

    }

    private void Update() {

        foreach(Boid b in boids) {
            b.OnUpdate();
        }

    }

}
