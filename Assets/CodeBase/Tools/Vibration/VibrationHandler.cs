using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace CodeBase.Tools.Vibration
{
	public class VibrationHandler : MonoBehaviour
	{
		#region Singleton

		private static VibrationHandler _instance;

		public static VibrationHandler Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<VibrationHandler>();

					if (_instance == null)
					{
						var go = new GameObject("[Vibration Handler]");
						DontDestroyOnLoad(go);
						_instance = go.AddComponent<VibrationHandler>();
					}
				}

				return _instance;
			}
		}

		#endregion

		private readonly List<MyHapticTypes> _addedVibrationPerFrame = new();

		private readonly Dictionary<int, HapticPatterns.PresetType> _overrideHapticsType =
			new()
			{
				{(int) MyHapticTypes.LightImpact, HapticPatterns.PresetType.LightImpact},
				{(int) MyHapticTypes.MediumImpact, HapticPatterns.PresetType.MediumImpact},
				{(int) MyHapticTypes.HeavyImpact, HapticPatterns.PresetType.HeavyImpact},
				{(int) MyHapticTypes.RigidImpact, HapticPatterns.PresetType.RigidImpact},
				{(int) MyHapticTypes.SoftImpact, HapticPatterns.PresetType.SoftImpact},
				{(int) MyHapticTypes.Selection, HapticPatterns.PresetType.Selection},
				{(int) MyHapticTypes.Failure, HapticPatterns.PresetType.Failure},
			};

		private bool _isCanPlayVibro;

		private readonly MyHapticTypes[] _orderHaptic =
		{
			MyHapticTypes.LightImpact,
			MyHapticTypes.MediumImpact,
			MyHapticTypes.HeavyImpact,
			MyHapticTypes.RigidImpact,
			MyHapticTypes.SoftImpact,
			MyHapticTypes.Selection,
			MyHapticTypes.Failure,
		};

		public void AddVibration(MyHapticTypes hapticTypes)
		{
			_addedVibrationPerFrame.Add(hapticTypes);
			_isCanPlayVibro = true;
		}

		private void TryPlayVibration()
		{
			if (!_isCanPlayVibro) return;

			_isCanPlayVibro = false;

			PlayVibration(CalculateVibrationTypeByOrder());
		}

		private void PlayVibration(MyHapticTypes hapticType)
		{
			if (hapticType == MyHapticTypes.Selection)
			{
				HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
			}
			else
			{
				if (_overrideHapticsType.TryGetValue((int) hapticType, out var value))
				{
					HapticPatterns.PlayPreset(value);
				}
			}

			_addedVibrationPerFrame.Clear();
		}

		private MyHapticTypes CalculateVibrationTypeByOrder()
		{
			var maxOrder = int.MinValue;

			foreach (var vibro in _addedVibrationPerFrame)
			{
				for (int j = 0; j < _orderHaptic.Length; j++)
				{
					if (vibro == _orderHaptic[j] && maxOrder < j)
					{
						maxOrder = j;
					}
				}
			}

			return _orderHaptic[maxOrder];
		}

		private void LateUpdate() => TryPlayVibration();
	}
}