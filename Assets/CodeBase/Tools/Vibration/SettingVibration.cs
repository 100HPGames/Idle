using UnityEngine;

namespace CodeBase.Tools.Vibration
{
	public static class SettingVibration
	{
		private const string Vibration = "Vibration";

		public static bool VibrationEnabled
		{
			get => PlayerPrefs.GetInt(Vibration, 1) == 1;
			set => PlayerPrefs.SetInt(Vibration, value ? 1 : 0);
		}
	}
}