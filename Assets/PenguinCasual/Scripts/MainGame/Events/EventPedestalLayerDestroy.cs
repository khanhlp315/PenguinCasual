namespace Penguin
{
    public struct EventPedestalLayerDestroy : IEvent
    {
        public EventPedestalLayerDestroy(PedestalLayer layer)
        {
            this.layer = layer;
        }

        public PedestalLayer layer;
    }
}