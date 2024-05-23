using UnityEngine;

public class AppSetting : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = -1;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
