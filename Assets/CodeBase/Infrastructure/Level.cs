using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public Transform PlayerSpawn { get; set; }
        [field: SerializeField] public Transform EnemySpawn { get; set; }
    }
}