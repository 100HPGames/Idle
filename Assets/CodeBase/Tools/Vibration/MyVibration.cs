namespace CodeBase.Tools.Vibration
{
	public static class MyVibration
	{
		private static bool _isCanPlayVibro;

		public static void Haptic(MyHapticTypes hapticType)
		{
			if (SettingVibration.VibrationEnabled == false) return;
			VibrationHandler.Instance.AddVibration(hapticType);
		}
	}
}