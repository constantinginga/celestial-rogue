using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShipWreckController : MonoBehaviour
{
    public Rigidbody2D[] Parts; 
	public List<Sprite> Sprites = new List<Sprite>();
    public Texture2D texture;
    int counter = 0;

    public void CreateWreck(string chosenSpaceship){
        foreach(Sprite sprite in Sprites){
            if(sprite.name.Substring(0, sprite.name.Length - 2).Equals(chosenSpaceship) && counter <= 3){
                Parts[counter].GetComponent<SpriteRenderer>().sprite = sprite;
                Parts[counter].AddForce(new Vector2(Parts[counter].transform.position.x - this.transform.position.x,Parts[counter].transform.position.y - this.transform.position.y) * 1.2F);
                counter++;
            }
        }
    }
}
