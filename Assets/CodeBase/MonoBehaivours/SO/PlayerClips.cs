using UnityEngine;

namespace CodeBase.MonoBehaivours.SO
{
    [CreateAssetMenu(menuName = "SO/PlayerClips", fileName = "PlayerClips")]
    public class PlayerClips : ScriptableObject
    {
        [field: SerializeField] public AnimationClip BaseAttack { get; set; }
        [field: SerializeField] public AnimationClip UltAttack { get; set; }
        [field: SerializeField] public AnimationClip Hit { get; set; }
        [field: SerializeField] public AnimationClip Idle { get; set; }
        [field: SerializeField] public AnimationClip Die { get; set; }
        [field: SerializeField] public AnimationClip Win { get; set; }
        [field: SerializeField] public AnimationClip Run { get; set; }
    }
}