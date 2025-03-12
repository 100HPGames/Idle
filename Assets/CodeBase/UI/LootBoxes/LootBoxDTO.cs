using System;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.LootBoxShop;
using MoneyHandler;

namespace _Tools
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