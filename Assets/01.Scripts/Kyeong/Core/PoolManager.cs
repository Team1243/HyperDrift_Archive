using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static PoolManager Instance;

    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    private Transform _trmParent;
    public PoolManager(Transform trmParent)
    {
        _trmParent = trmParent;
    }

    public void CreatePool(IPoolable prefab, int count = 10)
    {
        Pool pool = new Pool(prefab, count, _trmParent);
        _pools.Add(prefab.gameObject.name, pool); //프리팹의 이름으로 풀을 만든다.
    }

    public IPoolable Pop(string prefabName)
    {
        if(!_pools.ContainsKey(prefabName))
        {
            Debug.LogError($"Prefab does not exist on pool : {prefabName}");
            return null;
        }

        IPoolable item = _pools[prefabName].Pop();
        item.Initialize();
        return item;
    }

    public void Push(IPoolable obj)
    {
        string str = obj.gameObject.name.Replace(" (Clone)", ""); 
        _pools[str].Push(obj);
    }
}