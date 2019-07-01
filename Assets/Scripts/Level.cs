using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

public class Level : MonoBehaviour
{
	private const Single PIPE_WIDTH = 10f;
	private const Single PIPE_HEAD_HEIGHT = 6f;
	private const Single PIPE_HEAD_WIDTH = 10f;
	private const Single CAMERA_ORTHOGRAPHIC_SIZE = 50f;
	private const Single DESTROY_PIPE_X_POSITION = -115f;
	private const Single SPAWN_PIPE_X_POSITION = 115f;
	private const Single PIPE_MOVEMENT_SPEED = 0.5f;
	private const Single BIRD_X_POSITION = 0f;

    private Single _edgeHeight = 10f;

    private Single _timeElapsedFromSpawn;
	private Single _spawnInterval;
	private Single _currentGapSize;
	private Int32 _pipesSpawned;
	private Int32 _currentScore;
	private EState _currentState;

	public event Action<Int32> OnScoreIncreased;  

	[SerializeField]
	private GameObject _pipeHead;
	[SerializeField]
	private GameObject _pipeBody;

	private List<Pipe> _pipes;
	private Pipe _nearestPipe;

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
		_pipesSpawned++;
		SetDifficulty(GetDifficulty());
	}

	private void SetDifficulty(EDifficulty difficulty)
	{
		switch (difficulty)
		{
            case EDifficulty.Easy:
				_currentGapSize = 50f;
				_spawnInterval = 1.2f;
				break;
            case EDifficulty.Medium:
				_currentGapSize = 40f;
				_spawnInterval = 1.1f;
				break;
            case EDifficulty.Hard:
				_currentGapSize = 30f;
				_spawnInterval = 1.0f;
				break;
            case EDifficulty.Impossible:
				_currentGapSize = 20f;
				_spawnInterval = 0.9f;
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
		}
	}

	private EDifficulty GetDifficulty()
	{
		if (_pipesSpawned >= 30)
			return EDifficulty.Impossible;

		if (_pipesSpawned >= 20)
			return EDifficulty.Hard;

		if (_pipesSpawned >= 10)
			return EDifficulty.Medium;

		return EDifficulty.Easy;
	}

	private void Awake()
	{
		_pipes = new List<Pipe>();
		_spawnInterval = 1.0f;
		_currentGapSize = 50f;
		_currentState = EState.Playing;
	}

    private void Start()
	{
		InitialSpawning();
        FindObjectOfType<Bird>().OnDied += Bird_OnDied;
	}

    private void Bird_OnDied(Object sender, EventArgs e)
	{
		_currentState = EState.BirdIsDead;
	}

    //private void FixedUpdate()
    //{
    //    HandlePipesMovement();
    //}

    private void Update()
	{
		if (_currentState == EState.Playing)
		{
			HandlePipesSpawning();
			HandlePipesMovement();
			HandlePipesDestruction();
			HandleScoreCount();
		}
	}

    //private void LateUpdate()
    //{
    //	HandlePipesDestruction();
    //}

	private void HandleScoreCount()
	{
		if (_nearestPipe.GetX() < BIRD_X_POSITION)
		{
            //Increase score
			_currentScore++;
			OnScoreIncreased?.Invoke(_currentScore);

			//then
            foreach (var pipe in _pipes)
			{
				if (pipe.GetX() > BIRD_X_POSITION)
				{
					if (_nearestPipe.GetX() < BIRD_X_POSITION)
					{
						_nearestPipe = pipe;
						continue;
					}

					if (pipe.GetX() < _nearestPipe.GetX())
						_nearestPipe = pipe;
                }
            }
		}
	}

	private void InitialSpawning()
	{
		SpawnPipes();
		_nearestPipe = _pipes[0];
	}

	private void HandlePipesSpawning()
	{
		_timeElapsedFromSpawn += Time.deltaTime;

		if (_timeElapsedFromSpawn >= _spawnInterval)
		{
			_timeElapsedFromSpawn = 0;

			SpawnPipes();
		}
    }

	private void SpawnPipes()
	{
		Single minHeight = _currentGapSize * 0.5f + _edgeHeight;
		Single maxHeight = CAMERA_ORTHOGRAPHIC_SIZE * 2f - _currentGapSize * 0.5f - _edgeHeight;
		Single height = UnityEngine.Random.Range(minHeight, maxHeight);
		CreatePipes(height, _currentGapSize, SPAWN_PIPE_X_POSITION);
    }

    private void HandlePipesMovement()
	{
		foreach (var pipe in _pipes)
		{
			//pipe.Move(Vector3.left * PIPE_MOVEMENT_SPEED * Time.deltaTime);
			pipe.Move(Vector3.left * PIPE_MOVEMENT_SPEED);
		}
	}

	private void HandlePipesDestruction()
	{
		for (int i = _pipes.Count - 1; i >= 0; i--)
		{
			if (_pipes[i].GetX() < DESTROY_PIPE_X_POSITION)
			{
				var pipeToRemove = _pipes[i];
				pipeToRemove.DestroySelf();
				_pipes.Remove(pipeToRemove);
			}
		}
    }
}

public enum EDifficulty
{
	Easy,
	Medium,
	Hard,
	Impossible,
}

public enum EState
{
	BirdIsDead,
	Playing,
}
