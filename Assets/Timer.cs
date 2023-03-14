using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	public int currentTime;
	float converter = 0.0F;
	public bool stopped;
	TMP_Text UI;
	
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
}
