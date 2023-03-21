using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyPink : MonoBehaviour
{
    public int Health = 10;
    public float speed = 5;
    public EnemyController enemyController;
    private Transform target;
    public float FiringDistance = 3F;
    
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyController.Health = Health;
        enemyController.Heathbar.maxValue = Health;
        enemyController.Heathbar.value = Health;
        enemyController.GetComponent<AIPath>().maxSpeed = speed;
        target = enemyController.target.target;
    }
    
    void Update(){
        if (target && isInFiringDistance())
        {
            CreateAOE();
            Destroy(enemyController.transform.parent.gameObject);
        }
    }
    
    bool isInFiringDistance()
    {
        return Vector2.Distance(transform.position, target.position) <= FiringDistance;
    }
    void CreateAOE()
    {
        GameObject AOE = Resources.Load("Prefabs/AOE") as GameObject;
        AOE.GetComponent<SpriteRenderer>().color = Color.red;
        AOE.GetComponent<AOEController>().Damage = 30;
        AOE.GetComponent<AOEController>().SpeedOfGrowth = 5f;
        Instantiate(AOE, transform.position, Quaternion.identity);
    }
}
