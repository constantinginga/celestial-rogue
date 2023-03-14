using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	public int currentTime;
	public bool stopped;
	public TMP_Text UI;
	float converter = 0.0F;
	
	void Awake(){
		UI = GetComponent<TMP_Text>();
		stopped = false;
	}
	
    void Update()
	{
		if(!stopped){
			converter += Time.deltaTime;
			currentTime = (int)converter;
			UI.text = currentTime.ToString();
		}
	}
    
	public void Reset(){
		stopped = true;
		currentTime = 0;
		converter = 0.0F;
		UI.text = currentTime.ToString();
		UI.enabled = false;
	}
}
