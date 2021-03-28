using UnityEngine;

public class FPSLimit : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
