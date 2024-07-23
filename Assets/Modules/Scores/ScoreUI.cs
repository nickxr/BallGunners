using TMPro;
using UnityEngine;
using VContainer;

namespace Scores
{
    public class ScoreUI : MonoBehaviour
    {
        public TextMeshProUGUI playerScoreText;
        public TextMeshProUGUI opponentScoreText;

        private ServerScore _serverScore;

        [Inject]
        private void Construct(ServerScore serverScore)
        {
            _serverScore = serverScore;
            _serverScore.IDInstalledEvent += Initialize;
        }

        private void Initialize()
        {
            Debug.Log($"_serverScore.myTeamId {_serverScore.myTeamId}");
            if (_serverScore.myTeamId == 1)
            {
                _serverScore.UpdateTeam0ScoreUI += UpdateOpponentScore;
                _serverScore.UpdateTeam1ScoreUI += UpdatePlayerScore;
            }
            else
            {
                _serverScore.UpdateTeam0ScoreUI += UpdatePlayerScore;
                _serverScore.UpdateTeam1ScoreUI += UpdateOpponentScore;
            }
        }

        private void UpdatePlayerScore(int newValue)
        {
            playerScoreText.text = newValue.ToString();
        }
        private void UpdateOpponentScore(int newValue)
        {
            opponentScoreText.text = newValue.ToString();
        }

        private void OnDestroy()
        {
            if (_serverScore == null) return;
            _serverScore.IDInstalledEvent -= Initialize;

            if (_serverScore.myTeamId == 1)
            {
                _serverScore.UpdateTeam0ScoreUI -= UpdateOpponentScore;
                _serverScore.UpdateTeam1ScoreUI -= UpdatePlayerScore;
            }
            else
            {
                _serverScore.UpdateTeam0ScoreUI -= UpdatePlayerScore;
                _serverScore.UpdateTeam1ScoreUI -= UpdateOpponentScore;
            }
        }
    }
}