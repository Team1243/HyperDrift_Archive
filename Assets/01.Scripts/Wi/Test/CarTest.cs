using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
	[SerializeField] private Transform visual;
	[SerializeField] private float acceleration = 50f;
	[SerializeField] private float maxSpeed = 30f;

	private Quaternion targetRot;
	private Vector3 screenCenter;
	private RaycastHit hitInfo;
	private bool isGrounded = false;

	private Rigidbody rigid;
	
	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		screenCenter = new Vector3(Screen.width, Screen.height) * 0.5f;
	}

	private void SetRotation()
	{
		Vector3 direction = Input.mousePosition - screenCenter;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		targetRot = Quaternion.Euler(0, 90, 0) * Quaternion.Euler(0, -angle, 0);
	}

	private void CheckGrounded()
	{
		isGrounded = Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1.1f);
		if (isGrounded)
		{
			Vector3 toDir = new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z);
			transform.rotation = Quaternion.FromToRotation(Vector3.up, toDir);
		}
		else
		{
			rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y - 10f, rigid.velocity.z);
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.001f);
		}
	}

	private void Update()
	{
		//CheckGrounded();
		SetRotation();
		ControllSpeed();
		visual.localRotation = targetRot; 
	}

	private void ControllSpeed()
	{
		//if (!isGrounded) return;

		Vector3 vel = rigid.velocity;

		if (vel.magnitude > maxSpeed)
		{
			Vector3 clampedVel = vel.normalized * maxSpeed;
			rigid.velocity = clampedVel;
		}
	}

	private void FixedUpdate()
	{
		//if (isGrounded)
		{
		rigid.AddForce(visual.forward * acceleration * Time.fixedDeltaTime);
		}
	}
}