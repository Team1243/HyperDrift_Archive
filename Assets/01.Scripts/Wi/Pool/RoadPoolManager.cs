using System.Collections.Generic;
using UnityEngine;

public class RoadPoolManager
{
    private RoadCollectionSO roadCollection;
	private Transform parentTrm;
	private int makePoolCount = 2;
    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

	public RoadPoolManager(RoadCollectionSO roadCollection, Transform parentTrm, int makePoolCount)
	{
		this.roadCollection = roadCollection;
		this.parentTrm = parentTrm;
		this.makePoolCount = makePoolCount;
	}

	//public static RoadPoolManager Instance { get; private set; }

	//private void Awake()
	//{
	//	if (Instance == null)
	//	{
	//		Instance = this;
	//	}
	//	else
	//	{
	//		Destroy(gameObject);
	//		return;
	//	}
	//	MakePools();
	//}

	public void MakePools()
	{
		{
			Pool pool = new Pool(roadCollection.roadBehind, makePoolCount, parentTrm);
			pools.Add(roadCollection.roadBehind.gameObject.name, pool);
			pool = new Pool(roadCollection.startSection, makePoolCount, parentTrm);
			pools.Add(roadCollection.startSection.gameObject.name, pool);
		}
		foreach (RoadSection rs in roadCollection.sections)
		{
			if (pools.ContainsKey(rs.gameObject.name)) continue;
			Pool pool = new Pool(rs, makePoolCount, parentTrm);
			pools.Add(rs.gameObject.name, pool);
		}	
	}

	public IPoolable Pop(string key)
	{
		IPoolable obj = null;
		if (pools.ContainsKey(key))
		{
			obj = pools[key].Pop();
		}
		else
		{
			Debug.LogWarning($"No Pool: {key}");
		}
		return obj;
	}

	public void Push(IPoolable obj)
	{
		if (pools.ContainsKey(obj.gameObject.name))
		{
			pools[obj.gameObject.name].Push(obj);
		}
		else
		{
			Debug.LogWarning($"No Pool: {obj.gameObject.name}");
		}
	}
}
