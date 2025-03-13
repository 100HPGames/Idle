using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.LevelProgressService;

namespace CodeBase.Tools.StaticDataLoader
{
	[Serializable]
	public class GameDTO
	{
		public PlayerDTO PlayerDto;
		public ShopDTO ShopDto;
		public List<EnemyDTO> EnemyList;
		public List<LocationDTO> LocationsList;
	}
}