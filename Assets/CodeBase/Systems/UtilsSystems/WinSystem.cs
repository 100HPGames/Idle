using CodeBase.Components.Interactions;
using CodeBase.Infrastructure;
using CodeBase.Tools.SimpleMessenger;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace CodeBase.Systems.UtilsSystems
{
	public class WinSystem : IProtoRunSystem
	{
		[DI] private GameAspect _aspect;
		[DI] private Messenger _messenger;

		public void Run()
		{
			foreach (var entity in _aspect.WinsIt)
			{
				_messenger.Pub(new EndLevelMessage() {Win = true, Soft = 100, Hard = 100});
				_aspect.Wins.Del(entity);
			}
		}
	}
}