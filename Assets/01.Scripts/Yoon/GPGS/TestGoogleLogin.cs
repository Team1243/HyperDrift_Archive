/*using GooglePlayGames;
using UnityEngine;

public class TestGoogleLogin : MonoBehaviour
{
    private void Start()
    {
        LogIn();
    }

    public void LogIn()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                Debug.Log("Login Fail");
            }
            else
            {
                Debug.Log("Login Success");
            }
        });
    }

}
*/