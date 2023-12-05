using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Road/Collection")]
public class RoadCollectionSO : ScriptableObject
{
    public RoadBehind roadBehind;
    public RoadSection startSection;
    public List<RoadSection> sections;
}
