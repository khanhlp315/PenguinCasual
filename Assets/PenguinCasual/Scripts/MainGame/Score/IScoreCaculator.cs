﻿namespace Penguin
{
    public interface IScoreCaculator
    {
        event System.Action<long, long> OnScoreUpdate;

        long Score { get; }

        void OnPassingLayer(bool hasPowerup, PedestalLayer pedestalLayer);
        void OnLandingLayer(bool hasCombo, int comboCount, Pedestal pedestalLayer);
        void Update(float timeDelta);
        void PreventUpdateScoreOnNextLanding();
    }
}