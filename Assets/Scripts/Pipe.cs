using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe
{
	private readonly Transform _pipeHead;
	private readonly Transform _pipeBody;

	private Pipe(Transform pipeHead, Transform pipeBody)
	{
		_pipeHead = pipeHead;
		_pipeBody = pipeBody;
	}

	public static Pipe Create(Transform pipeHead, Transform pipeBody) => new Pipe(pipeHead, pipeBody);

	public void Move(Vector3 move)
	{
		_pipeHead.position += move;
		_pipeBody.position += move;
	}

	public Single GetX() => _pipeHead.position.x;

	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(_pipeHead.transform.gameObject);
		UnityEngine.Object.Destroy(_pipeBody.transform.gameObject);
	}
}
