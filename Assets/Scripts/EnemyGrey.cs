using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGrey : MonoBehaviour
{
    public int Health = 15;
	public float FiringDistance = 50F;
    public float FiringCooldown = 15F;
    public float BeamScaleIncrement = 0.5F;
    public float BeamCooldown = 10F;
	public bool isFiring;
	bool canFire;
    EnemyController enemy;
    AIPath pathFinding;
    FacePlayer facePlayer;
	Transform lockedTargetPosition;
	Rigidbody2D rigidBody2D;
	Transform player;

	void Awake(){
		rigidBody2D = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyController>();
        pathFinding = GetComponent<AIPath>();
		facePlayer = GetComponent<FacePlayer>();
		player = GameObject.FindFirstObjectByType<PlayerController>().transform;
        pathFinding.endReachedDistance = FiringDistance;
		canFire = true;
		isFiring = false;
    }

    void Update()
    {
	    if(enemy.target.target && isInFiringDistance(transform.position, enemy.target.target.position)){
	    	if(canFire){
	    		isFiring = true;
		    	LockTargetPosition();
		    	StartFiring();
		    	StartFiringCooldown();
	    	}
	    	else if(!canFire && !isFiring){
		    	rigidBody2D.constraints = RigidbodyConstraints2D.FreezePosition;
		    	facePlayer.lockedTarget = true;
		    	facePlayer.facingDirection = player.position;
	    	}
	    }
	    else{
	    	rigidBody2D.constraints = RigidbodyConstraints2D.None;
		    StopFiring();
		    enemy.target.target = player;
	    }
    }

	bool isInFiringDistance(Vector2 origin, Vector2 target){
        return Vector2.Distance(origin, target) <= FiringDistance;
    }

    void LockTargetPosition(){
	    rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void StartFiringCooldown(){
        canFire = false;
		Invoke("ClearFiringCooldown", FiringCooldown);
    }
	
	void ClearFiringCooldown(){
		canFire = true;
	}

    void StartFiring(){
        CreateBeam();
    }

    void StopFiring(){
	    facePlayer.lockedTarget = false;
    }

    void CreateBeam(){
        GameObject beam = Instantiate(Resources.Load("Prefabs/Beam", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity);
	    beam.transform.SetParent(transform);
        beam.transform.localRotation = Quaternion.Euler(0,0,-90);
        beam.transform.GetChild(0).localScale = new Vector3(0,0.1F,0.1F);
        StartCoroutine(IncrementBeam(beam));
    }

    IEnumerator IncrementBeam(GameObject beam){
        while(beam.transform.GetChild(0).localScale.x <= 50){
            beam.transform.GetChild(0).localScale = new Vector3(beam.transform.GetChild(0).localScale.x + BeamScaleIncrement,0.1F,0.1F);
            Destroy(beam, 5F);
            yield return new WaitForSeconds(0.005F);
        }
    }
}
