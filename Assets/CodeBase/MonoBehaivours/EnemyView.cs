using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.ProgressBars;
using UnityEngine;

namespace CodeBase.MonoBehaivours
{
	public class EnemyView : MonoBehaviour
	{
		[Header("UI")]
		[SerializeField] private ProgressBar _healthBar;
		
		public void SetMaxHealth(float healthValue)
		{
			_healthBar.SetMaxValue(healthValue, true);
			_healthBar.SetValue(healthValue);
		}

		public void SetHealth(float healthValue)
		{
			_healthBar.SetValue(healthValue);
		}
	}
}