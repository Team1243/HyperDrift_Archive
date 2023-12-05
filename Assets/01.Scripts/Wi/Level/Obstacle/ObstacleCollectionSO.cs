using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Road/ObstacleCollection")]
public class ObstacleCollectionSO : ScriptableObject
{
    public List<GameObject> obstacles;

    public GameObject GetObstacle()
	{
		if (obstacles.Count == 0) return null;
		int index = Random.Range(0, obstacles.Count);
		return obstacles[index];
	}        
}
