using DG.Tweening;
using Mirror;
using Scores;
using UnityEngine;
using VContainer;

namespace Player
{
    public class GoalController : MonoBehaviour
    {
        public float minZ = -10f; // Минимальная позиция по оси Z
        public float maxZ = 10f;  // Максимальная позиция по оси Z
        public float moveDuration = 2f; // Время, необходимое для перемещения от minZ до maxZ

        private Sequence _sequence;
        
        public bool isPlayerGoal = true;
        private ScoreManager _scoreManager;

        [Inject]
        public void Construct(ScoreManager scoreManager)
        {
            Debug.Log("GoalController Construct");
            _scoreManager = scoreManager;
        }
        
        private void Start()
        {
            isPlayerGoal = transform.root.GetComponent<NetworkIdentity>().isLocalPlayer;
            // Запускаем анимацию перемещения
            StartMovement();
        }

        private void StartMovement()
        {
            // Создаем последовательность для анимации
            _sequence = DOTween.Sequence();

            // Определяем начальную и конечную позиции
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, minZ);
            Vector3 endPosition = new Vector3(transform.position.x, transform.position.y, maxZ);

            // Устанавливаем начальное положение объекта
            transform.position = startPosition;

            // Перемещение объекта от minZ до maxZ
            _sequence.Append(transform.DOMoveZ(maxZ, moveDuration)
                .SetEase(Ease.Linear));

            // Пауза перед обратным движением
            _sequence.AppendInterval(0.5f);

            // Перемещение объекта от maxZ до minZ
            _sequence.Append(transform.DOMoveZ(minZ, moveDuration)
                .SetEase(Ease.Linear));

            // Пауза перед следующим циклом
            _sequence.AppendInterval(0.5f);

            // Повторяем анимацию бесконечно
            _sequence.SetLoops(-1, LoopType.Restart);
        }

        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter: {other.gameObject.name}");

            if (other.CompareTag("Ball"))
            {
                if (isPlayerGoal)
                {
                    _scoreManager.SubtractScore(true);  // Уменьшение счета игрока
                }
                else
                {
                    _scoreManager.AddScore(true);  // Увеличение счета игрока
                }

                Destroy(other.gameObject);  // Удалить мяч
            }
        }

        private void OnDestroy()
        {
            // Останавливаем все анимации, связанные с этим объектом
            if (_sequence != null)
            {
                _sequence.Kill();
            }
        }
    }
}