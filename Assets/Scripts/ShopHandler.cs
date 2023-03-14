using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
	public Button VentureForthButton; 
	GameManager GameManager;
 
	void Awake(){
		GameManager = GameObject.FindFirstObjectByType<GameManager>();
	}
    
	public void VentureForth(){
		GameManager.StartLevel();
	}
}
