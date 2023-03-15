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
        if(!lockedTarget){
            FaceVelocity();
        }
    }

    void FaceVelocity()
    {
        facingDirection = aIPath.desiredVelocity;
        if (facingDirection.x != 0 && facingDirection.y != 0)
        {
            transform.up = facingDirection * -1.0f;
        }
    }
}
