using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FacePlayer : MonoBehaviour
{
    public Vector2 facingDirection;
    public Rigidbody2D RigidBody;
    public AIPath aIPath;
    public bool lockedTarget;

    void Awake(){
        lockedTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
            FaceVelocity();
    }

    void FaceVelocity()
	{
		if(!lockedTarget){
        	facingDirection = aIPath.desiredVelocity;
			if (facingDirection.x != 0 && facingDirection.y != 0)
			{
				transform.up = facingDirection * -1.0f;
			}
		}
		else{
			Vector2 direction = facingDirection - new Vector2(transform.position.x, transform.position.y);
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90F;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		}
    }
}
