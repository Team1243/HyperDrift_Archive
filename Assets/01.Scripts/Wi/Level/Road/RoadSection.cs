using UnityEngine;

public class RoadSection : MonoBehaviour, IPoolable
{
    [SerializeField] private Transform enterPos;
    public Transform EnterPosition => enterPos;
    [SerializeField] private Transform exitPos;
    public Transform ExitPosition => exitPos;
    [SerializeField] private DirectionInfo exitDir;
    public DirectionInfo ExitDir => exitDir;
    [SerializeField] private Vector3 size;
    public Vector3 Size => size;

    private ObstacleManager obstacle;

	private void Awake()
	{
		obstacle = transform.Find("Decorations")?.GetComponent<ObstacleManager>();
	}

	public void Initialize()
	{
        obstacle?.Initialize();
	}
}
