using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QeustListSO", menuName = "SO/Quest/QuestListSO")]
public class QuestInfoListSO : ScriptableObject
{
    public List<Quest> QuestList = new List<Quest>();
}
