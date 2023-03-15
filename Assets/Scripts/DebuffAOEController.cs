using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAOEController : MonoBehaviour
{
 [Header("Attributes")]
    public int SlowdownEffect = 1;
    public float LengthOfEffect = 0.5F;
    [Header("Components")]
    public LayerMask PlayerLayer;
    private float originalSpeed;
    void Awake(){
        originalSpeed = 0;
        Activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer){
            originalSpeed = collision.GetComponent<PlayerController>().speed;
            collision.gameObject.SendMessage("Slowdown", SlowdownEffect);
        }
    }

    private void OnTriggerStay2D(Collider2D collision){
        if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer){
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
        Destroy(gameObject);
    }
}
