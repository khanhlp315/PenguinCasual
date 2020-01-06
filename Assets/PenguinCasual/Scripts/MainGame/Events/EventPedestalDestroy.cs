namespace Penguin
{
    public struct EventPedestalDestroy : IEvent
    {
        public EventPedestalDestroy(Pedestal pedestal)
        {
            this.pedestal = pedestal;
        }

        public Pedestal pedestal;
    }
}