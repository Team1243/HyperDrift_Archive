using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Poolpair
{
    public GameObject prefab;
    public int count = 2;
} 

[CreateAssetMenu(menuName = "SO/PoolableList")]
public class PoolableListSO : ScriptableObject
{
    public List<Poolpair> poolables = new List<Poolpair>();
}
