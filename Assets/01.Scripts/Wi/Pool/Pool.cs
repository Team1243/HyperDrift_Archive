using System.Collections.Generic;
using UnityEngine;

public class Pool
{
	private IPoolable prefab;
    private Stack<IPoolable> pool = new Stack<IPoolable>();
	private Transform parent;

    public Pool(IPoolable obj, int count, Transform parent)
	{
		prefab = obj;
		this.parent = parent;

		for (int i = 0; i < count; ++i)
		{
			IPoolable instance = GameObject.Instantiate(prefab.gameObject, parent).GetComponent<IPoolable>();
			instance.gameObject.name = instance.gameObject.name.Replace("(Clone)", "");
			Push(instance);
		}
	}

	public void Push(IPoolable obj)
	{
		obj.gameObject.SetActive(false);
		pool.Push(obj);
	}

	public IPoolable Pop()
	{
		IPoolable obj;
		if (pool.Count == 0)
		{
			obj = GameObject.Instantiate(prefab.gameObject, parent).GetComponent<IPoolable>();
			obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");
		}
		else
		{
			obj = pool.Pop();
		}
		obj.Initialize();
		obj.gameObject.SetActive(true);
		return obj;
	}
}
