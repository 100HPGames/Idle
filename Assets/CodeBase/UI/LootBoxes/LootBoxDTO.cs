using System;
using CodeBase.UI.Mediators;

namespace CodeBase.UI.LootBoxes
{
	[Serializable]
	public class LootBoxDTO
	{
		public LootBoxType LootBoxType; 
		public int RewardsCount;
		public CurrencyType PriceType;
		public int PriceValue;
	}
}