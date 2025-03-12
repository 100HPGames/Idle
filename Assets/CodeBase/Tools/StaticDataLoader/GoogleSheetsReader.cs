using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using Leopotam.GoogleDocs.Unity;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Tools.StaticDataLoader
{
	public class GoogleSheetsReader : MonoBehaviour
	{
		private Dictionary<string, string> _tagsCsvMap;

		[Button]
		public void LoadData() => ReadGameData();

		private async Task ReadCsvData(string pageId)
		{
			(Dictionary<string, string> csv, string err)
				= await GoogleDocsLoader.LoadFullPage(pageId, GameConst.DividerTag);
			if (err != default)
				Debug.Log(err);
			else
				_tagsCsvMap = csv;
		}

		private async void ReadGameData()
		{
			GameDTO gameDto = new GameDTO();

			await ReadCsvData(GameConst.PlayerDTO);
			gameDto.PlayerDto = ParsePlayerDTO();
			Debug.Log("parse PlayerDto successfully");

			await ReadCsvData(GameConst.ShopDTO);
			gameDto.ShopDto = ParseShopDTO();
			Debug.Log("parse ShopDTO successfully");

			await ReadCsvData(GameConst.EnemiesDTO);
			gameDto.EnemyList = ParseEnemiesDTO();
			Debug.Log("parse EnemyList successfully");

			await ReadCsvData(GameConst.LevelsDTO);
			gameDto.LocationsList = ParseLocationsDTO();
			Debug.Log("parse LocationsList successfully");

			ParseHelper.SaveObjectToResourcesInJson(gameDto, GameConst.GameDataFileName);
		}

		private PlayerDTO ParsePlayerDTO()
		{
			Dictionary<string, string> keyedValues = ParseHelper.GetKeyedValues(_tagsCsvMap[GameConst.BaseStat]);
			return (PlayerDTO) ParseHelper.WriteDataInObjectByType(typeof(PlayerDTO), keyedValues, _tagsCsvMap);
		}

		private ShopDTO ParseShopDTO()
		{
			Dictionary<string, string> keyedValues = ParseHelper.GetKeyedValues(_tagsCsvMap[GameConst.BaseStat]);
			return (ShopDTO) ParseHelper.WriteDataInObjectByType(typeof(ShopDTO), keyedValues, _tagsCsvMap);
		}
		
		private List<EnemyDTO> ParseEnemiesDTO()
		{
			Dictionary<string, List<string>> baseStatsMap
				= ParseHelper.GetKeyedListsByOwner(_tagsCsvMap[GameConst.BaseStat]);
			return (List<EnemyDTO>) ParseHelper.WriteDataInListByType(typeof(EnemyDTO), baseStatsMap, _tagsCsvMap);
		}

		private List<LocationDTO> ParseLocationsDTO()
		{
			Dictionary<string, List<string>> baseStatsMap
				= ParseHelper.GetKeyedListsByOwner(_tagsCsvMap[GameConst.BaseStat]);
			return (List<LocationDTO>) ParseHelper.WriteDataInListByType(typeof(LocationDTO), baseStatsMap, _tagsCsvMap);
		}
	}
}