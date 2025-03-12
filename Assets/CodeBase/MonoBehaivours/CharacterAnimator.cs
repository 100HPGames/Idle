using System.Linq;
using CodeBase.Tools.StaticDataLoader;
using UnityEngine;

namespace CodeBase.MonoBehaivours
{
    public abstract class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;

        private const string ToRun = "ToRun";
        private const string ToAttack = "ToAttack";
        private const string ToWin = "ToWin";
        private const string ToDead = "ToDead";
        private const string ToHit = "ToHit";
        private const string AttackSpeed = "AttackSpeed";
        protected const string Idle = "Idle";
        protected const string Attack = "Attack";
        protected const string Die = "Die";
        protected const string Hit = "Hit";
        protected const string Run = "Run";
        protected const string Win = "Win";

        private readonly int _runBool = Animator.StringToHash(ToRun);
        private readonly int _attackBool = Animator.StringToHash(ToAttack);
        private readonly int _winBool = Animator.StringToHash(ToWin);
        private readonly int _deadBool = Animator.StringToHash(ToDead);
        private readonly int _hitTrigger = Animator.StringToHash(ToHit);
        private readonly int _attackSpeed = Animator.StringToHash(AttackSpeed);

        protected AnimatorOverrideController _animatorOverrideController;
        protected AnimationClipOverrides _clipOverrides;

        protected virtual void Awake()
        {
            _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = _animatorOverrideController;
            _clipOverrides = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
            _animatorOverrideController.GetOverrides(_clipOverrides);
            _clipOverrides[Attack] = SetAttack();
            _clipOverrides[Idle] = SetIdle();
            _clipOverrides[Hit] = SetHit();
            _clipOverrides[Die] = SetDie();
            _clipOverrides[Run] = SetRun();
            _clipOverrides[Win] = SetWin();
            _animatorOverrideController.ApplyOverrides(_clipOverrides);
        }

        public virtual float PlayAttack(float animationDuration)
        {
            _clipOverrides[Attack] = SetAttack();
            _animatorOverrideController.ApplyOverrides(_clipOverrides);
            AnimationClip clip = _clipOverrides[Attack];
            AnimationEvent hitEvent = clip.events.FirstOrDefault(e => e.functionName == GameConst.MakeAttack);
            float speedMultiplier = clip.length / animationDuration;
            float newEventTime = hitEvent?.time / speedMultiplier ?? animationDuration / 2;
            _animator.SetFloat(_attackSpeed, speedMultiplier);
            _animator.SetBool(_attackBool, true);
            return newEventTime;
        }
        public virtual void StopAttack() => _animator.SetBool(_attackBool, false);
        public virtual void PlayHit() => _animator?.SetBool(_hitTrigger, true);
        public virtual void PlayRun() => _animator.SetBool(_runBool, true);
        public virtual void StopRun() => _animator.SetBool(_runBool, false);

        public virtual void PlayDeath()
        {
            _animator.SetBool(_attackBool, false);
            _animator.SetBool(_deadBool, true);
        }

        public virtual void PlayWin() => _animator.SetBool(_winBool, true);
        
        protected virtual AnimationClip SetAttack() => default;
        protected virtual AnimationClip SetHit() => default;
        protected virtual AnimationClip SetDie()=> default;
        protected virtual AnimationClip SetIdle() => default;
        protected virtual AnimationClip SetRun() => default;
        protected virtual AnimationClip SetWin() => default;
    }
}