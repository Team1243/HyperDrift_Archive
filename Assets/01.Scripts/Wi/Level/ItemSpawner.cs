using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject timerPrefab;
	[SerializeField] private ObstacleCollectionSO obstacleCollection;

    private RoadGenerationController rgc;
	private Queue<GameObject> itemQueue = new Queue<GameObject>();

	private Vector3 lastTimerSpawnPos;
	private Vector3 lastObstacleSpawnPos;

	[SerializeField]
	private float minimumTimerSpawnDistance = 100f;
	[SerializeField]
	private float minimumObstaclepawnDistance = 100f;
	[SerializeField][Range(0, 1f)]
	private float obstacleSpawnRatio = 0.5f;

	private void Awake()
	{
		rgc = GetComponent<RoadGenerationController>();
		rgc.OnCreateRoad += OnCreateRoadHandle;
	}

	private void OnCreateRoadHandle(RoadSection road, int roadCount)
	{
		if (roadCount < 1) return;

		//bool fuelCreated = CreateFuel(road);
		//if (!fuelCreated)
		//{
			float percentage = UnityEngine.Random.Range(0, 1f);
			if (percentage <= obstacleSpawnRatio)
			{
				CreateObstacle(road);
			}
		//}

		if (itemQueue.Count > 0)
		{
			try
			{
				GameObject item = itemQueue.Peek();

				if (item == null)
				{
					itemQueue.Dequeue();
				}
				else if (Vector3.Distance(item.transform.position, road.ExitPosition.position) > 2000.0f)
				{
					itemQueue.Dequeue();
					Destroy(item.gameObject);
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"The Fuel you trying to access is destroyed. [ {e.Message} ]");
			}
		}

	}

	private bool CreateFuel(RoadSection road)
	{
		if (Vector3.Distance(lastTimerSpawnPos, road.ExitPosition.position) < minimumTimerSpawnDistance) 
			return false;

		GameObject instance = Instantiate(timerPrefab, road.ExitPosition.position + Vector3.up * 1, Quaternion.identity);
		itemQueue.Enqueue(instance);
		lastTimerSpawnPos = instance.transform.position;

		return true;
	}

	private void CreateObstacle(RoadSection road)
	{
		if (Vector3.Distance(lastObstacleSpawnPos, road.ExitPosition.position) < minimumObstaclepawnDistance)
			return;
		Quaternion rotation = Quaternion.Euler(0, ((int)rgc.RoadDirection <  16 ? 0 : (int)rgc.RoadDirection < 32 ? 90f : -90f), 0);

		GameObject obstacle = obstacleCollection?.GetObstacle();
		if (obstacle != null)
		{
			GameObject instance = Instantiate(obstacle, road.ExitPosition.position, rotation);
			itemQueue.Enqueue(instance);
			lastObstacleSpawnPos = instance.transform.position;
		}
	}
}
