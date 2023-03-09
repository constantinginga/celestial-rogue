using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AteroidSpawner : MonoBehaviour
{
    [SerializeField] private AsteroidScript asteroidPrefab;
    [SerializeField] private float spawnRate = 2.0f;
    [SerializeField] private int spawnAmountMoving = 1;
    [SerializeField] private int spawnAmountStationary = 5;
    [SerializeField] private float spawnDistance = 50.0f;
    [SerializeField] private float trajectoryVariance = 15.0f;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        InvokeRepeating(nameof(spawn), this.spawnRate, this.spawnRate );
        spawnStationary();
    }

    private void spawn()
    {
        for (int i = 0; i < this.spawnAmountMoving; i++)
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

    private void spawnStationary()
    {
        for (int i = 0; i < spawnAmountStationary; i++)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-50, 50),Random.Range(-20, 20),0);
            
            Collider[] colliders = Physics.OverlapSphere(spawnPoint, 0.1f, layerMask);

            if (colliders.Length == 0)
            {
                AsteroidScript asteroid = Instantiate(this.asteroidPrefab, spawnPoint, this.transform.rotation);
                asteroid.size = asteroid.maxSize + 1f;
            }
        }
    }

}
