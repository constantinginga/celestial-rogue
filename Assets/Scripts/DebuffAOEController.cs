using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAOEController : MonoBehaviour
{
 [Header("Attributes")]
    public int SlowdownEffect = 1;
    public float LengthOfEffect = 0.5F;
	float originalSpeed;
    [Header("Components")]
    public LayerMask PlayerLayer;
	PlayerController player;
	bool deactivating;
	
    void Awake(){
	    originalSpeed = 0;
	    deactivating = false;
	    player = FindFirstObjectByType<PlayerController>();
        Activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer && !deactivating){
            originalSpeed = collision.GetComponent<PlayerController>().speed;
            collision.gameObject.SendMessage("Slowdown", SlowdownEffect);
        }
    }

    private void OnTriggerStay2D(Collider2D collision){
	    if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer && !deactivating){
            collision.gameObject.SendMessage("Slowdown", SlowdownEffect);
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
      if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer){
            collision.gameObject.SendMessage("Slowdown", originalSpeed);
        }
    }

    void Activate()
	{
		Invoke("Deactivate", LengthOfEffect);
	}

	void Deactivate(){
		deactivating = true;
		CircleCollider2D col = GetComponent<CircleCollider2D>();
		if(col.IsTouchingLayers(7)){
			player.speed = originalSpeed;
		}
        Destroy(gameObject);
    }
}
