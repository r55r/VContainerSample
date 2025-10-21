using System;

namespace State
{
    [Serializable]
    public sealed class GameState
    {
        public ClickState clickState = new();
        public UpgradeState upgradeState = new();
        public CoinState coinState = new();
    }
}