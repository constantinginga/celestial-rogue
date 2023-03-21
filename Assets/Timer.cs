using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	public int currentTime;
	public bool stopped;
	public TMP_Text UI;
	float converter;
	
	void Awake(){
		UI = GetComponent<TMP_Text>();
	}
	
    void Update()
	{
		if(!stopped){
			converter -= Time.deltaTime;
			currentTime = (int)converter;
			UI.text = currentTime.ToString();
		}
	}
    public void Start(){
		converter = GameObject.FindFirstObjectByType<GameManager>().CurrentLevelLength;
		stopped = false;
	}
	public void Reset(){
		currentTime = (int)GameObject.FindFirstObjectByType<GameManager>().CurrentLevelLength;
		converter = GameObject.FindFirstObjectByType<GameManager>().CurrentLevelLength;
		UI.text = currentTime.ToString();
		UI.enabled = false;
	}
}
