using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Tools.StateMachine
{
	public class StateMachine
	{
		public event Action<string, string> OnChangeState;
		private IState _currentState;
		private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
		private List<Transition> _currentTransitions = new List<Transition>();
		private readonly List<Transition> _anyTransitions = new List<Transition>();
		private static List<Transition> EmptyTransitions = new List<Transition>(0);

		public void Tick()
		{
			_currentState.Tick();
			Transition transition = GetTransition();
			if (transition != default)
				SetState(transition.To);
		}
		
		public void ClearTransitions()
		{
			_currentTransitions.Clear();
			_anyTransitions.Clear();
			
			foreach (var tPair in _transitions)
				tPair.Value.Clear();
			
			_transitions.Clear();
		}

		public void LogState() => Debug.Log(_currentState.GetType().Name);

		public void SetState(IState state)
		{
			if (state == _currentState)
				return;
			
			OnChangeState?.Invoke(_currentState?.GetType().Name, state.GetType().Name);
			
			_currentState?.OnExit();
			_currentState = state;

			_transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
			_currentTransitions ??= EmptyTransitions;

			_currentState.OnEnter();
		}

		public void AddTransition(IState from, IState to, Func<bool> predicate)
		{
			if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
			{
				transitions = new List<Transition>();
				_transitions[from.GetType()] = transitions;
			}

			transitions.Add(new Transition(to, predicate));
		}

		public void AddAnyTransition(IState state, Func<bool> predicate)
		{
			_anyTransitions.Add(new Transition(state, predicate));
		}

		private Transition GetTransition()
		{
			foreach (var transition in _currentTransitions)
				if (transition.Condition())
					return transition;

			foreach (var transition in _anyTransitions)
				if (transition.Condition())
					return transition;

			return default;
		}

		private class Transition
		{
			public Func<bool> Condition { get; }
			public IState To { get; }

			public Transition(IState to, Func<bool> condition)
			{
				To = to;
				Condition = condition;
			}
		}
	}
}