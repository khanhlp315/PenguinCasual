namespace Penguin
{
    public struct EventUpdateScore : IEvent
    {
        public EventUpdateScore(long score, long increase)
        {
            this.score = score;
            this.increase = increase;
        }

        public long score;
        public long increase;
    }
}