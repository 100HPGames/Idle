using CodeBase.Tools.StaticDataLoader;
using UnityEngine;

namespace CodeBase.MonoBehaivours
{
	public abstract class EnemyAbstract : MonoBehaviour
	{
		public GameObject GameObject => gameObject;
		public Transform Transform => transform;

		public virtual void SetMaxHealth(float healthValue)
		{
		}

		public virtual void SetHealth(float healthValue)
		{
		}
	}
}