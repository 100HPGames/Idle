using System;
using CodeBase.UI.Mediators;

namespace CodeBase.Tools.StaticDataLoader
{
	[Serializable]
	public class CurrencyShopDTO
	{
		public CurrencyType CurrencyType;
		public Rarity Rare;
		public float Count;
		public float Price;
		public CurrencyType PriceType;
	}
}