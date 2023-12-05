using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
	public float speed = 0.5f;
    private float scrollX;
    private float scrollY;

	private Material mat;

	private float offsetX;
	private float offsetY;

	private void Awake()
	{
		mat = GetComponent<Renderer>().material;
		float angle = transform.rotation.z - 90f * Mathf.PI / 180f;
		scrollX = Mathf.Cos(angle);
		scrollY = Mathf.Sin(angle);
		mat.mainTextureScale = new Vector2(1f, transform.localScale.z / transform.localScale.x);
	}

	private void Update()
	{
		offsetX = Time.time * scrollX * speed;
		offsetY = Time.time * scrollY * speed;
		mat.mainTextureOffset = new Vector2(offsetX, offsetY);
	}
}
