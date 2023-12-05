using UnityEngine;

public class SampleObjQuest : MonoBehaviour
{
    [SerializeField] private Quest quest;

    public int apple = 100;
    public int Apple
    {
        get => apple;
        set
        {
            apple = value;
            // quest.JobAct(Condition());
        }
    }

    private void Start()
    {
        quest.OnRunning();

        Apple = 35;
        // quest.JobAct(Condition());
    }

    [ContextMenu("UPDATE")]
    public void Test()
    {
        quest.JobAct(Condition());
    }

    [ContextMenu("INIT")]
    public void Test2()
    {
        quest.JobInit();
    }

    public bool Condition()
    {
        if (Apple >= 30)
        {
            return true;    
        }
        else
        {
            return false;
        }
    }
}
