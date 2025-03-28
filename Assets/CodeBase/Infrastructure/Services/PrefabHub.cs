﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.MonoBehaivours;
using CodeBase.Tools.StaticDataLoader;
using CodeBase.UI.FlyService;
using CodeBase.UI.LootBoxes;
using CodeBase.UI.MainMenu.Shop.CurrencyShop;
using UnityEngine;

namespace CodeBase.Infrastructure.Services
{
	[CreateAssetMenu(menuName = "SO/PrefabHub", fileName = "PrefabHub")]
	public class PrefabHub : ScriptableObject
	{
		
		[SerializeField] private List<EnemyPrefab> _enemyPrefabs;
		[field: SerializeField] public PlayerView PlayerView { get; set; }
		[field: SerializeField] public FlyItemView FlyItemView { get; set; } 
		[field: SerializeField] public CurrencyElementPlankView CurrencyElementPlankView { get; set; }
		[field: SerializeField] public DamageView DamageViewPrefab { get; set; }
		[field: SerializeField] public List<LootBoxOpenView> LootBoxes { get; set; }
		
		public EnemyView GetEnemyView(EnemyId enemyId)
		{
			var prefab = _enemyPrefabs.FirstOrDefault(x => x.Id == enemyId);
			return prefab?.View ?? _enemyPrefabs[0].View;
		}
	}

	[Serializable]
	public class EnemyPrefab
	{
		public EnemyId Id;
		public EnemyView View;
	}
}