using System;
using System.Collections.Generic;

namespace Penguin
{
    public static class EventHub
    {
        private static List<IEventQueue> _eventQueues;

        static EventHub()
        {
            _eventQueues = new List<IEventQueue>();
            Runner.ScheduleLateUpdate(Dispatch);
        }

        static EventQueue<T> EQ<T>() where T : struct, IEvent
        {
            int index = EventType<T>.Index;
            if (index == -1)
            {
                TypeManager<IEvent>.RegisterType(typeof(T));
                index = EventType<T>.SyncIndex();
                _eventQueues.Add(new EventQueue<T>());
            }

            return (EventQueue<T>)_eventQueues[index];
        }

        public static void Bind<T>(Action<T> method, bool receiveImmediately = false) where T : struct, IEvent
        {
            EQ<T>().Bind(method, receiveImmediately);
        }

        public static void Bind<T>(Action<T[], int> method, bool receiveImmediately = false) where T : struct, IEvent
        {
            EQ<T>().Bind(method, receiveImmediately);
        }

        public static void Unbind<T>(Action<T> method) where T : struct, IEvent
        {
            EQ<T>().Unbind(method);
        }

        public static void Unbind<T>(Action<T[], int> method) where T : struct, IEvent
        {
            EQ<T>().Unbind(method);
        }

        public static void Emit<T>(T e = default(T)) where T : struct, IEvent
        {
            EQ<T>().Emit(e);
        }

        public static void Emit<T>(T[] events) where T : struct, IEvent
        {
            EQ<T>().Emit(events);
        }

        public static bool HasEventInQueue<T>() where T : struct, IEvent
        {
            return EQ<T>().HasEventInQueue();
        }

        private static void Dispatch()
        {
            foreach (var eventQueue in _eventQueues)
            {
                eventQueue.Dispatch();
            }
        }

        public static void ClearAll()
        {
            foreach (var eventQueue in _eventQueues)
            {
                eventQueue.ClearAll();
            }
        }
    }
}