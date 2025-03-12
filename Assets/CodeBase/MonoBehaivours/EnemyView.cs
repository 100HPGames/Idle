using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.ProgressBars;
using UnityEngine;

namespace CodeBase.MonoBehaivours
{
	public class EnemyView : EnemyAbstract
	{
		[Header("UI")]
		[SerializeField] private ProgressBar _healthBar;

		private Coroutine _timerRoutine;

		public override void SetMaxHealth(float healthValue)
		{
			_healthBar.SetMaxValue(healthValue, true);
			_healthBar.SetValue(healthValue);
		}

		public override void SetHealth(float healthValue)
		{
			_healthBar.SetValue(healthValue);
		}
	}
}