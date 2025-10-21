using JetBrains.Annotations;
using State;
using UnityEngine;

namespace Service
{
    [UsedImplicitly]
    public sealed class GameStateSerializer : IGameStateSaver
    {
        const string PrefsKey = "GameState";

        GameState _gameState;

        public GameState LoadGameState()
        {
            var json = PlayerPrefs.GetString(PrefsKey, string.Empty);
            _gameState = !string.IsNullOrWhiteSpace(json) ? JsonUtility.FromJson<GameState>(json) : new();
            return _gameState;
        }

        public void SaveGameState()
        {
            var json = JsonUtility.ToJson(_gameState);
            PlayerPrefs.SetString(PrefsKey, json);
            PlayerPrefs.Save();
        }
    }
}