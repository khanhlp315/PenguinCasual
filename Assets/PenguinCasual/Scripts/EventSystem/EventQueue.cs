using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    internal interface IEventQueue
    {
        void Dispatch();
    }

    internal class EventQueue<T> : IEventQueue where T : struct, IEvent
    {
        private const int kDefaultEventSize = 2;

        private T[] _events;
        private T[] _tempEvents;
        private int _totalEvent;
        private List<Action<T[], int>> _batchSubscribers;
        private List<Action<T[], int>> _batchInsantSubscribers;
        private List<Action<T>> _oneSubscribers;
        private List<Action<T>> _oneInstantSubscribers;

        public int TotalEvent => _totalEvent;
        public T[] Events => _events;

        public EventQueue()
        {
            _totalEvent = 0;
            _events = new T[kDefaultEventSize];
            _tempEvents = new T[1];

            _batchSubscribers = new List<Action<T[], int>>();
            _batchInsantSubscribers = new List<Action<T[], int>>();
            _oneSubscribers = new List<Action<T>>();
            _oneInstantSubscribers = new List<Action<T>>();
        }

        public void Bind(Action<T[], int> subscriber, bool receiveImmediately = false)
        {
            if (receiveImmediately)
                _batchInsantSubscribers.Add(subscriber);
            else
                _batchSubscribers.Add(subscriber);
        }

        public void Bind(Action<T> subscriber, bool receiveImmediately = false)
        {
            if (receiveImmediately)
                _oneInstantSubscribers.Add(subscriber);
            else
                _oneSubscribers.Add(subscriber);
        }

        public void Unbind(Action<T[], int> subscriber)
        {
            _batchSubscribers.Remove(subscriber);
            _batchInsantSubscribers.Remove(subscriber);
        }

        public void Unbind(Action<T> subscriber)
        {
            _oneSubscribers.Remove(subscriber);
            _oneInstantSubscribers.Remove(subscriber);
        }

        public void Emit(T e = default(T))
        {
            if (_totalEvent == _events.Length)
            {
                Array.Resize(ref _events, _totalEvent * 2);
            }

            _events[_totalEvent++] = e;

            for (int i = 0, totalSub = _oneInstantSubscribers.Count; i < totalSub; i++)
            {
                _oneInstantSubscribers[i].Invoke(e);
            }

            _tempEvents[0] = e;
            for (int i = 0, totalSub = _batchInsantSubscribers.Count; i < totalSub; i++)
            {
                _batchInsantSubscribers[i].Invoke(_tempEvents, 1);
            }
        }

        public void Emit(T[] events)
        {
            int totalEmitEvent = events.Length;
            EnsureCapacity(totalEmitEvent + _totalEvent);

            for (int i = 0, totalSub = _oneInstantSubscribers.Count; i < totalSub; i++)
            {
                for (int j = 0; j < totalEmitEvent; j++)
                {
                    _oneInstantSubscribers[i].Invoke(events[j]);
                }
            }

            for (int i = 0, totalSub = _batchInsantSubscribers.Count; i < totalSub; i++)
            {
                _batchInsantSubscribers[i].Invoke(events, totalEmitEvent);
            }

            for (int i = 0; i < totalEmitEvent; i++)
            {
                _events[_totalEvent + i] = events[i];
            }

            _totalEvent += totalEmitEvent;
        }

        public bool HasEventInQueue()
        {
            return _totalEvent > 0;
        }

        public void Dispatch()
        {
            // Most of the time
            if (_totalEvent == 0)
                return;

            for (int i = 0, totalSub = _oneSubscribers.Count; i < totalSub; i++)
            {
                for (int j = 0; j < _totalEvent; j++)
                {
                    _oneSubscribers[i].Invoke(_events[j]);
                }
            }

            for (int i = 0, totalSub = _batchSubscribers.Count; i < totalSub; i++)
            {
                _batchSubscribers[i].Invoke(_events, _totalEvent);
            }

            _totalEvent = 0;
        }

        private void EnsureCapacity(int capacity, int power = 2)
        {
            int currentCapacity = _events.Length;
            if (currentCapacity < capacity)
            {
                if (currentCapacity == 0)
                    currentCapacity = 1;

                do
                {
                    currentCapacity *= power;
                }
                while (currentCapacity > capacity);

                Array.Resize(ref _events, currentCapacity);
            }
        }
    }
}