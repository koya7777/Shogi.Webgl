using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�X�̐F��ύX����
/// </summary>
public class SquareChangeColor : MonoBehaviour
{
	public int x;
	public int y;
	// Use this for initialization
	void Start()
	{
		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(1, 0, 0, 0.3f);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
