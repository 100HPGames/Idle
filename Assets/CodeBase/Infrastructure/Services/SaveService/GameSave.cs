using System;
using System.Collections.Generic;
using CodeBase.UI.Mediators;

namespace CodeBase.Infrastructure.Services.SaveService
{
	[Serializable]
	public class GameSave
	{
		//Currency
		public Dictionary<CurrencyType, int> CurrencyMap = new();
	}
}