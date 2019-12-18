using UnityEngine;

namespace Penguin
{
    public struct EventTouchBegan : IEvent
    {
        public Vector2 pos;
        public EventTouchBegan(Vector2 pos) => this.pos = pos;
    }

    public struct EventTouchMoved : IEvent
    {
        public Vector2 pos;
        public Vector2 deltaPos;
        public Vector2 startPos;
        public EventTouchMoved(Vector2 pos, Vector2 deltaPos, Vector2 startPos) => (this.pos, this.deltaPos, this.startPos) = (pos, deltaPos, startPos);
    }

    public struct EventTouchEnded : IEvent
    {
        public Vector2 pos;
        public Vector2 startPos;
        public EventTouchEnded(Vector2 pos, Vector2 startPos) => (this.pos, this.startPos) = (pos, startPos);
    }

    public struct EventTouchCanceled : IEvent
    {
        public Vector2 pos;
        public EventTouchCanceled(Vector2 pos) => this.pos = pos;
    }
}