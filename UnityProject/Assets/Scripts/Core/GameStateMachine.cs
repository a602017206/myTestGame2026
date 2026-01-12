using System;
using System.Collections.Generic;

namespace Core
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> states = new();
        private IGameState currentState;

        public void Register<TState>(TState state) where TState : IGameState
        {
            states[typeof(TState)] = state;
        }

        public void Start<TState>() where TState : IGameState
        {
            ChangeState<TState>();
        }

        public void ChangeState<TState>() where TState : IGameState
        {
            currentState?.Exit();
            currentState = GetState<TState>();
            currentState.Enter();
        }

        public void Tick()
        {
            currentState?.Tick();
        }

        private TState GetState<TState>() where TState : IGameState
        {
            if (!states.TryGetValue(typeof(TState), out IGameState state))
            {
                throw new InvalidOperationException($"State {typeof(TState).Name} is not registered.");
            }

            return (TState)state;
        }
    }
}
