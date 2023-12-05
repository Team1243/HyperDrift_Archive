using UnityEngine;

public class PlayerSystemObj : MonoBehaviour
{
    private void Awake()
    {
        var obj = FindObjectsOfType<PlayerSystemObj>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
