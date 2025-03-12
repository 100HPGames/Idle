using CodeBase.Components;
using CodeBase.Tools.StaticDataLoader;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace CodeBase.Systems.ViewSystems
{
    public class DamageTextMoveSystem : IProtoRunSystem
    {
        [DI] private GameAspect _gameAspect;

        public void Run()
        {
            foreach (var entity in _gameAspect.DamageDisplayIt)
            {
                ref var damageView = ref _gameAspect.DamageDisplay.Get(entity);
             
                if (IsReachedTarget(ref damageView) == false)
                {  
                    MoveDamageView(ref damageView);
                }
                else
                {
                    Object.Destroy(damageView.View.gameObject);
                    _gameAspect.DamageDisplay.Del(entity);
                }
            }
        }

        private bool IsReachedTarget(ref DamageDisplay proj)
        {
            Vector3 targetPosition = proj.StartPosition + new Vector3(0, GameConst.DamageViewDistance, 0f);
            return Vector3.Distance(proj.View.transform.position, targetPosition) < GameConst.DistanceThreshold;
        }

        private void MoveDamageView(ref DamageDisplay damageView)
        {
            damageView.Transition += Time.deltaTime / damageView.Duration;
            damageView.Transition = Mathf.Clamp01(damageView.Transition);

            Vector3 startPosition = damageView.StartPosition;
            Vector3 targetPosition = damageView.StartPosition + new Vector3(0, GameConst.DamageViewDistance, 0f);
            
            damageView.View.transform.position = Vector3.Lerp(startPosition, targetPosition, damageView.Transition);
        }
    }
}