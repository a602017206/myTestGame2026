namespace Core
{
    public class BootState : IGameState
    {
        private readonly GameStateMachine stateMachine;

        public BootState(GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            stateMachine.ChangeState<GenerateProfileState>();
        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }
    }
}
