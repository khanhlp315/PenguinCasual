namespace Penguin
{
    public struct EventEndGame : IEvent
    {
        public EventEndGame(bool isDead)
        {
            this.isCharacterDeath = isDead;
        }

        public bool isCharacterDeath;
    }
}
