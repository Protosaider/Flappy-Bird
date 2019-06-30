using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	private const Single PIPE_WIDTH = 10f;
	private const Single PIPE_HEAD_HEIGHT = 6f;
	private const Single PIPE_HEAD_WIDTH = 10f;
	private const Single CAMERA_ORTHOGRAPHIC_SIZE = 50f;
	private const Single DESTROY_PIPE_X_POSITION = -115f;
	private const Single PIPE_MOVEMENT_SPEED = 3f;

    [SerializeField]
	private GameObject _pipeHead;
	[SerializeField]
	private GameObject _pipeBody;

	private List<Pipe> _pipes;

	private void CreatePipe(Single height, Single x, Boolean isBottom)
	{
		Transform pipeHead = Instantiate(_pipeHead).transform;

		Single pipeHeadY = -CAMERA_ORTHOGRAPHIC_SIZE + height;
		pipeHeadY = isBottom ? pipeHeadY - PIPE_HEAD_HEIGHT : -pipeHeadY;
		pipeHead.position = new Vector3(x, pipeHeadY);


		Transform pipeBody = Instantiate(_pipeBody).transform;

		Single pipeBodyY = -CAMERA_ORTHOGRAPHIC_SIZE;
		pipeBodyY = isBottom ? pipeBodyY : -pipeBodyY - height;
		pipeBody.position = new Vector3(x, pipeBodyY);

		SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
		pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

		BoxCollider2D pipeBodyBoxCollider2D = pipeBody.GetComponent<BoxCollider2D>();
		pipeBodyBoxCollider2D.size = new Vector2(PIPE_WIDTH, height);
		pipeBodyBoxCollider2D.offset = Vector2.up * height * 0.5f;

		_pipes.Add(Pipe.Create(pipeHead, pipeBody));
	}

	private void CreatePipes(Single gapCenterY, Single gapSize, Single x)
	{
		CreatePipe(gapCenterY - gapSize * 0.5f, x, true);
		CreatePipe(CAMERA_ORTHOGRAPHIC_SIZE * 2 - gapCenterY - gapSize * 0.5f, x, false);
	}

    // Start is called before the first frame update
    void Start()
    {
        CreatePipe(40f, 0f, true);
        CreatePipe(40f, 0f, false);

		CreatePipes(50f, 20f, 20f);
	}

    // Update is called once per frame
    void Update()
	{
		HandlePipesMovement();
	}

	private void HandlePipesMovement()
	{
		foreach (var pipe in _pipes)
		{
			pipe.Move(Vector3.left * PIPE_MOVEMENT_SPEED * Time.deltaTime);
		}
	}
}
