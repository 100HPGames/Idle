using CodeBase.Components;
using CodeBase.Components.CharactersData;
using CodeBase.Components.Interactions;
using CodeBase.Components.States;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Systems
{
	sealed class GameAspect : ProtoAspectInject
	{
		public readonly ProtoPool<PlayerInput> PlayerInputs;
		public readonly ProtoPool<Enemy> Enemies;
		public readonly ProtoPool<Player> Players;
		public readonly ProtoPool<DamageDisplay> DamageDisplay;
		public readonly ProtoPool<MakeDamage> MakeDamage;
		public readonly ProtoPool<Health> Health;
		public readonly ProtoPool<Damage> Damages;
		public readonly ProtoPool<CharacterView> CharacterViews;
		public readonly ProtoIt PlayersIt = new(It.Inc<Player>());
		public readonly ProtoIt PlayerInputsIt = new(It.Inc<PlayerInput>());
		public readonly ProtoItExc AliveEnemiesIt = new(It.Inc<Enemy>(), It.Exc<Death>());
		public readonly ProtoIt DamageDisplayIt = new(It.Inc<DamageDisplay>());
		
		//states

		public readonly ProtoPool<Death> Death;
		public readonly ProtoPool<StartAttack> StartAttack;
		public readonly ProtoPool<StartMove> StartMove;
		public readonly ProtoPool<MakeAttack> MakeAttack;
		public readonly ProtoPool<ReloadAttack> ReloadAttack;
		public readonly ProtoPool<LooseGame> LooseGame;
		public readonly ProtoPool<WinGame> Wins;
		public readonly ProtoPool<CurrentState> CurrentStates;
		public readonly ProtoPool<InterruptedState> InterruptedStates;
		public readonly ProtoPool<NextState> NextStates;
		public readonly ProtoIt SwitchStatesIt = new(It.Inc<CurrentState, NextState>());
		public readonly ProtoIt EnemyStatesIt = new(It.Inc<CurrentState, Enemy>());
		public readonly ProtoIt PlayerStatesIt = new(It.Inc<CurrentState, Player>());
		public readonly ProtoIt LooseIt = new(It.Inc<LooseGame>());
		public readonly ProtoIt WinsIt = new(It.Inc<WinGame>());
	}
}