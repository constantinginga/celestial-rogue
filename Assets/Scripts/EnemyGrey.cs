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
    bool canFire;
    EnemyController enemy;
    AIPath pathFinding;
    FacePlayer facePlayer;
    Transform lockedTargetPosition;

    void Awake(){
        enemy = GetComponent<EnemyController>();
        pathFinding = GetComponent<AIPath>();
        facePlayer = GetComponent<FacePlayer>();
        pathFinding.endReachedDistance = FiringDistance;
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
