using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _currentScoreText;
	private String _scoreText = "Score: ";

    private void Awake()
	{
		//TODO: Better use FindWithTag
		//_currentScoreText = transform.Find("CurrentScore").GetComponent<Text>();
		_currentScoreText = transform.GetComponentInChildren<TextMeshProUGUI>();
		FindObjectOfType<Level>().OnScoreIncreased += OnScoreIncreased;
	}

	private void Start()
	{
		_currentScoreText.text = _scoreText + 0;
    }

    private void OnScoreIncreased(Int32 currentScore)
	{
		_currentScoreText.text = _scoreText + currentScore;
    }
}
