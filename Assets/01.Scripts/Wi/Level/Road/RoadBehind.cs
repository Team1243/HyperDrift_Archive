using UnityEngine;

public class RoadBehind : MonoBehaviour, IPoolable
{
    [SerializeField] private Transform center;
    public Transform Center => center;

	public void Initialize()
	{

	}
}
