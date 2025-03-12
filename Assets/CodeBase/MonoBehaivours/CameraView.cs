using UnityEngine;

namespace CodeBase.MonoBehaivours
{
    public class CameraView : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}