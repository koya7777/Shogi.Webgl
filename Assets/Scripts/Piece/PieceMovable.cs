using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 駒移動可能オブジェクト
/// </summary>
public class PieceMovable : MonoBehaviour {
	public Address Address;
	// Use this for initialization
	void Start () {
		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = new Color(0, 0, 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
