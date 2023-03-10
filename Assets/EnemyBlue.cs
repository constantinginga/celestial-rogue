using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBlue : MonoBehaviour
{
    public int Health = 20;
    public float DebuffCooldown = 10F;
    public float NewLocationSetCooldown = 5F;
    LayerMask CollisionLayer;
    bool CanDebuff;
    bool setNewTarget;
    EnemyController baseController;
    AIDestinationSetter target;

    void Awake(){
        baseController = GetComponent<EnemyController>();
        baseController.Health = Health;
        startDebuffCooldDown();
        setNewTarget = true;
        CollisionLayer = LayerMask.GetMask("Player","Enemy","Asteroid");
        target.target = null;
        target.target = chooseRandomLocation();
        startNewLocationCooldown();
    }

    void Update(){
        if(CanDebuff){
            CreateDebuffAOE();
            startDebuffCooldDown();
        }
        if(setNewTarget){
            target.target = chooseRandomLocation();
            startNewLocationCooldown();
        }
    }

    void CreateDebuffAOE(){
        Instantiate(Resources.Load ("Prefabs/DebuffAOE") as GameObject, transform.position, Quaternion.identity);
    }

    void startDebuffCooldDown()
	{
		CanDebuff = false;
		Invoke("clearDebuffCooldown", DebuffCooldown);
	}
	
	void clearDebuffCooldown(){
		CanDebuff = true;
	}

    void startNewLocationCooldown()
	{
		setNewTarget = false;
		Invoke("clearNewDebuffCooldown", DebuffCooldown);
	}
	
	void clearNewDebuffCooldown(){
		setNewTarget = true;
	}

    Transform chooseRandomLocation(){
        while(true){
                GameObject tmp = new GameObject();
                tmp.transform.position = new Vector3(Random.Range(-50F, 50F), Random.Range(-20F, 20F), 0);
                tmp.AddComponent<CircleCollider2D>();
                tmp.GetComponent<CircleCollider2D>().isTrigger = true;
                CircleCollider2D col = tmp.GetComponent<CircleCollider2D>();
		    if (!col.IsTouchingLayers(CollisionLayer))
		    {
                print(tmp.transform.position);
				return tmp.transform;
		    }
		}
    }



}
