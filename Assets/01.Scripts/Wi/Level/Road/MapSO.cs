using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Map")]
public class MapSO : ScriptableObject
{
    public Sprite mapImage;
    public string mapName;
    public string mapSceneName;
}
