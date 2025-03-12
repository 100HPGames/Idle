using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Tools.StaticDataLoader;
using UnityEngine;

namespace CodeBase.MonoBehaivours.SO
{
	[CreateAssetMenu(menuName = "SO/EnemyClips", fileName = "EnemyClips")]
	public class EnemyClips : ScriptableObject
	{
		[SerializeField] private List<EnemyTypeClips> _clips;

		public AnimationClip Attack(EnemyId id) => _clips.First(c=>c.Id==id).Attack;
		public AnimationClip Idle(EnemyId id) => _clips.First(c=>c.Id==id).Idle;
		public AnimationClip Hit(EnemyId id) => _clips.First(c=>c.Id==id).Hit;
		public AnimationClip Death(EnemyId id) => _clips.First(c=>c.Id==id).Death;
	}
    
	[Serializable]
    public class EnemyTypeClips
    {
	    public EnemyId Id;
	    public AnimationClip Idle;
	    public AnimationClip Attack;
	    public AnimationClip Hit;
	    public AnimationClip Death;
    }
}