using System;
using System.Collections.Generic;
using MoneyHandler;

namespace CodeBase.Infrastructure
{
	[Serializable]
	public class GameSave
	{
		//Currency
		public Dictionary<CurrencyType, int> CurrencyMap = new();
	}
}