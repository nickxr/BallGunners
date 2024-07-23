using System;
using DG.Tweening;
using Mirror;
using Network;
using Scores;
using UnityEngine;
using VContainer;

namespace Player
{
    public class GoalController : MonoBehaviour
    {
        public float minZ = -10f; 
        public float maxZ = 10f;  
        public float moveDuration = 2f;

        private Sequence _sequence;

        [SerializeField] private NetworkBallsSpawner networkBallsSpawner;

        public bool isPlayerGoal = true;
        private ServerScore _serverScore;
        private NetworkIdentity _parentInIdentity;
        public event Action IDInstalledEvent;

        [Inject]
        public void Construct(ServerScore serverScore)
        {
            Debug.Log("GoalController Construct");
            _serverScore = serverScore;
            IDInstalledEvent += _serverScore.OnInstallIID;
        }
        
        private void Start()
        {
            Transform root = transform.root;
            networkBallsSpawner ??= root.GetComponent<NetworkBallsSpawner>();
            
            _parentInIdentity = root.GetComponent<NetworkIdentity>();
            _serverScore.myTeamId = _parentInIdentity.isClientOnly ? 1 : 0;
            _serverScore.opponentTeamId = _serverScore.myTeamId == 1 ? 0 : 1;
            isPlayerGoal = _parentInIdentity.isLocalPlayer;
            IDInstalledEvent?.Invoke();
            
            StartMovement();
        }
        
        private void StartMovement()
        {
            _sequence = DOTween.Sequence();

            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y, minZ);

            transform.position = startPosition;

            // Move from minZ to maxZ
            _sequence.Append(transform.DOMoveZ(maxZ, moveDuration)
                .SetEase(Ease.Linear));

            // Pause
            _sequence.AppendInterval(0.5f);
            
            //Go back
            _sequence.Append(transform.DOMoveZ(minZ, moveDuration)
                .SetEase(Ease.Linear));

            _sequence.AppendInterval(0.5f);

            // always restart
            _sequence.SetLoops(-1, LoopType.Restart);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"{other.tag} OnTriggerEnter, isMine{isPlayerGoal}, myTeam {_serverScore.myTeamId}");
            if (!other.CompareTag("Ball")) return;
            _serverScore.AddScore(!isPlayerGoal ? _serverScore.myTeamId : _serverScore.opponentTeamId);
            
            if (_parentInIdentity.isServer)
            {
                networkBallsSpawner.HandleBallHit(other.gameObject);
            }
        }

        private void OnDestroy()
        {
            // Stop all tween animations
            if (_sequence != null)
            {
                _sequence.Kill();
            }

            if (_serverScore != null)
            {
                IDInstalledEvent -= _serverScore.OnInstallIID;
            }
        }
    }
}