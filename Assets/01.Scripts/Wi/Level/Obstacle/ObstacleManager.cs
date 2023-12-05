using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	private List<Obstacle> obstacles = new List<Obstacle>();

	private void Awake()
	{
		GetComponentsInChildren(obstacles);
	}

	public void Initialize()
	{
		foreach (Obstacle obstacle in obstacles)
		{
			obstacle?.Initialize();
		}
	}
}
