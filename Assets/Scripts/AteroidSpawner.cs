using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AteroidSpawner : MonoBehaviour
{
    [SerializeField] private AsteroidScript asteroidPrefab;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private int spawnAmount = 1;
    [SerializeField] private float spawnDistance = 50.0f;
    [SerializeField] private float trajectoryVariance = 15.0f;

    void Start()
    {
        InvokeRepeating(nameof(spawn), this.spawnRate, this.spawnRate );
    }

    private void spawn()
    {
        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
            

           AsteroidScript asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
           asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
           asteroid.setTrajectory(rotation * -spawnDirection);
        }
    }
   
}
