using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
	public float bulletSpeed = 50f;
	public int damageAmount = 10;
	EnemyGrey parent;
	
	void Start(){
		parent = GetComponentInParent<EnemyGrey>();
	}
	
	protected void OnDestroy()
	{
		GameObject bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, parent.transform.position, Quaternion.identity);
		bullet.transform.rotation = parent.transform.rotation;
		if (bullet.TryGetComponent<BulletController>(out BulletController bulletComponent))
		{
			bulletComponent.parentLayer = parent.gameObject.layer;
			bulletComponent.damageAmount = damageAmount;
			bulletComponent.opponentLayer = 192;
		}
		bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed * 5);
		parent.isFiring = false;
		Destroy(bullet, 3f);
	}
}
