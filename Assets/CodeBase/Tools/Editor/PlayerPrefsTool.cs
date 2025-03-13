using UnityEditor;
using UnityEngine;

namespace CodeBase.Tools.Editor
{
	public static class PlayerPrefsTool
	{
		[MenuItem("PlayerPrefs/Clear All")]
		public static void ClearAllPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}
	}
}