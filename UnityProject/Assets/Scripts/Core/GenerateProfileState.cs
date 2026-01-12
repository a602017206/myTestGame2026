namespace Core
{
    public class GenerateProfileState : IGameState
    {
        private readonly GameEntry gameEntry;
        private readonly GameStateMachine stateMachine;

        public GenerateProfileState(GameEntry gameEntry, GameStateMachine stateMachine)
        {
            this.gameEntry = gameEntry;
            this.stateMachine = stateMachine;
        }

        public void Enter()
        {
            gameEntry.GenerateAndStoreAffinity();
            stateMachine.ChangeState<MainMenuState>();
        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }
    }
}
