using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    // get this from the weapon somehow
	public int damageAmount = 1;
	public int opponentLayer = 1;

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((opponentLayer | (1 << collision.gameObject.layer)) == opponentLayer)
        {
    		collision.gameObject.SendMessage("TakeDamage", damageAmount);
            Destroy(gameObject);
        }
    }
}
