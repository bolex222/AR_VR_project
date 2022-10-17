namespace Interfaces
{
    public interface IGameBehaviour
    {
        public void GameStart();
        public void GameOver(AllGenericTypes.Team winner);
        public void SetUpGame();
    }
}