using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGrey : MonoBehaviour
{
    public int Health = 15;
    public float FiringDistance = 50F;
    public float FiringCooldown = 15F;
    public float BeamScaleIncrement = 0.05F;
     public float BeamCooldown = 10F;
    bool canFire;
    bool canStartBeamCooldown;
    EnemyController enemy;
    AIPath pathFinding;
    FacePlayer facePlayer;
    Transform lockedTargetPosition;
    GameObject Beam;

    void Awake(){
        enemy = GetComponent<EnemyController>();
        pathFinding = GetComponent<AIPath>();
        facePlayer = GetComponent<FacePlayer>();
        pathFinding.endReachedDistance = FiringDistance;
        canStartBeamCooldown = true;
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInFiringDistance(transform.position, enemy.target.target.position) && canFire){
            LockTargetPosition();
            StartFiring();
            StartFiringCooldown();
        }
        else{
            StopFiring();
            enemy.target.target = GameObject.FindFirstObjectByType<PlayerController>().transform;
        }
    }

    bool isInFiringDistance(Vector2 origin, Vector2 target){
        return Vector2.Distance(origin, target) <= FiringDistance;
    }

    void LockTargetPosition(){
        lockedTargetPosition =  enemy.target.target;
        facePlayer.lockedTarget = true;
        facePlayer.facingDirection = lockedTargetPosition.position;
    }

    void StartFiringCooldown(){
        canFire = false;
		Invoke("ClearFiringCooldown", FiringCooldown);
    }
	
	void ClearFiringCooldown(){
		canFire = true;
	}

    void StartBeamCooldown(){
        canStartBeamCooldown = false;
        Invoke("ClearBeamCooldown", BeamCooldown);
    }

    void ClearBeamCooldown(){
        canStartBeamCooldown = true;
        /* Destroy(Beam); */
    }

    void StartFiring(){
        //Lock tranform position
        print("Fire");
        CreateBeam();
    }

    void StopFiring(){
        facePlayer.lockedTarget = false;
        /* print("Stop Firing"); */
    }

    void CreateBeam(){
        GameObject beam = Instantiate(Resources.Load("Prefabs/Beam", typeof(GameObject)) as GameObject, transform.position, Quaternion.identity);
        Beam = beam;
        beam.transform.SetParent(transform);
        beam.transform.localRotation = Quaternion.Euler(0,0,0);
        beam.transform.GetChild(0).localScale = new Vector3(0,1,1);
        InvokeRepeating("IncrementBeam", 0, 0.1F);
    }

    void IncrementBeam(){
        Beam.transform.GetChild(0).localScale = new Vector3(Beam.transform.localScale.x + BeamScaleIncrement,1,1);
            if(Beam.transform.GetChild(0).localScale.x >= 1){
                Beam.transform.GetChild(0).localScale = new Vector3(1,1,1);
                if(canStartBeamCooldown){
                    StartBeamCooldown();    
                }
            }
    }
}
