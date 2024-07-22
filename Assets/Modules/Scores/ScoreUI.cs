using TMPro;
using UnityEngine;
using VContainer;

namespace Scores
{
    public class ScoreUI : MonoBehaviour
    {
        public TextMeshProUGUI playerScoreText;
        public TextMeshProUGUI opponentScoreText;

        private ScoreManager _scoreManager;

        [Inject]
        private void Construct(ScoreManager scoreManager)
        {
            Debug.Log("ScoreUI Construct");
            _scoreManager = scoreManager;
        }
        
        private void Start()
        {
            _scoreManager.AddPlayerScoreEvent += UpdateScoreDisplay;
        }
        
        private void UpdateScoreDisplay()
        {
            playerScoreText.text = _scoreManager.GetPlayerScore().ToString();
            opponentScoreText.text = _scoreManager.GetOpponentScore().ToString();
        }

        private void OnDestroy()
        {
            _scoreManager.AddPlayerScoreEvent -= UpdateScoreDisplay;
        }
    }
}