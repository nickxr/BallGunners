using System;

namespace Scores
{
    public class ScoreManager
    {
        private int _playerScore = 0;  // Текущий счет игрока
        private int _opponentScore = 0;  // Счет соперника

        public event Action AddPlayerScoreEvent;
        // Метод для увеличения счета
        public void AddScore(bool isPlayerScore)
        {
            AddPlayerScoreEvent?.Invoke();
            if (isPlayerScore)
            {
                _playerScore++;
            }
            else
            {
                _opponentScore++;
            }
        }

        // Метод для уменьшения счета
        public void SubtractScore(bool isPlayerScore)
        {
            if (isPlayerScore && _playerScore > 0)
            {
                _playerScore--;
            }
            _opponentScore++;
        }

        // Метод для получения счета игрока
        public int GetPlayerScore()
        {
            return _playerScore;
        }

        // Метод для получения счета соперника
        public int GetOpponentScore()
        {
            return _opponentScore;
        }
    }
}