using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEController : MonoBehaviour
{
    [Header("Attributes")]
    public float SpeedOfGrowth = 1.25F;
    public int Damage = 1;
    public float selfDestroyDelay = 0.5F;
    [Header("Components")]
    public Transform AOEAreaFill;
    public CircleCollider2D collider2D;
    public LayerMask PlayerLayer;
    public ParticleSystem Explosion;
    void Awake(){
        collider2D.enabled = false;
        AOEAreaFill.localScale = new Vector3(0.1F,0.1F,0.1F);
    }
    // Update is called once per frame
    void Update()
    {
        if(AOEAreaFill.localScale.x <= 1){
            AOEAreaFill.localScale = new Vector3(AOEAreaFill.localScale.x * SpeedOfGrowth, AOEAreaFill.localScale.y * SpeedOfGrowth, 0);
        }
        else{
            collider2D.enabled = true;
            startSelfDestroyDelay();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if((PlayerLayer | (1 << collision.gameObject.layer)) == PlayerLayer){
            collision.gameObject.SendMessage("TakeDamage", Damage);
        }
    }

    public void startSelfDestroyDelay()
	{
        Explosion.Play();
		Invoke("clearSelfDestroyCooldown", selfDestroyDelay);
	}
	
	public void clearSelfDestroyCooldown(){
		Destroy(gameObject);
	}
}
