using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure;
using CodeBase.Tools.SimpleMessenger;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using MoneyHandler;
using Object = UnityEngine.Object;

namespace CodeBase.Systems.LevelsProgression
{
	public class InitPlayerSystem : IProtoInitSystem
	{
		private readonly bool _isDummy;
		[DI] private GameAspect _aspect;
		[DI] private Messenger _messenger;
		[DI] private StaticData _staticData;
		[DI] private AssetProvider _assetProvider;

		public InitPlayerSystem(bool isDummy = false)
		{
			_isDummy = isDummy;
		}

		public void Init(IProtoSystems systems)
		{
			var prefab = _assetProvider.GetPlayerView();
			var playerDto = _staticData.GameDTO.PlayerDto;
			_aspect.Players.NewEntity(out var playerEntity);
			ref var view = ref _aspect.CharacterViews.Add(playerEntity);
			ref var health = ref _aspect.Health.Add(playerEntity);
			ref var currentState = ref _aspect.CurrentStates.Add(playerEntity);
			health.Value = playerDto.HeroHealth;
			health.MaxValue = playerDto.HeroHealth;
			view.View = Object.Instantiate(prefab);
			currentState.State = _isDummy ? StateType.Idle : StateType.Reload;
		}
	}
}