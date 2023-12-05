using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Pooling")]
    [SerializeField] private PoolableListSO _poolableList;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        PoolManager.Instance = new PoolManager(transform);
        _poolableList.poolables.ForEach(p => PoolManager.Instance.CreatePool(p.prefab.GetComponent<IPoolable>(), p.count));
    }
}
