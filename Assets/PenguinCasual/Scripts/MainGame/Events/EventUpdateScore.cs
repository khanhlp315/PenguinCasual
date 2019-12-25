namespace Penguin
{
    public struct EventUpdateScore : IEvent
    {
        public EventUpdateScore (long score)
        {
            this.score = score;
        }

        public long score;
    }
}