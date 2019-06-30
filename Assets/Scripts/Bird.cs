using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
	private const Single JUMP_VELOCITY_MULTIPLIER = 100f;

	private Rigidbody2D _rigidbody2D;

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			Jump();
		}
    }

	private void Jump()
	{
		_rigidbody2D.velocity = Vector2.up * JUMP_VELOCITY_MULTIPLIER;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		 
	}
}
