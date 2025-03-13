using CodeBase.UI.Mediators;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Tools.CurrencyHandler
{
	[CreateAssetMenu(fileName = "New CurrencyData", menuName = "Currency Data")]
	public class CurrencyData : ScriptableObject
	{
		[SerializeField] private CurrencyType _type;
		[ShowAssetPreview, SerializeField] private Sprite _resourceIcon;
		public CurrencyType Type => _type;
		public Sprite ResourceIcon => _resourceIcon;

#if UNITY_EDITOR
		[Button]
		private void ApplyName()
		{
			var assetPath = AssetDatabase.GetAssetPath(this);
			AssetDatabase.RenameAsset(assetPath, $"Currency_{_type}");
			AssetDatabase.Refresh();
		}
#endif
	}
}