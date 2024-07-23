using System;
using Mirror;
using UnityEngine;

namespace Scores
{
    public class ServerScore: NetworkBehaviour
    {
        public int myTeamId;
        public int opponentTeamId;
        public event Action<int> UpdateTeam0ScoreUI;
        public event Action<int> UpdateTeam1ScoreUI;
        [SyncVar(hook = nameof(OnTeam1ScoreChanged))]
        public int team1Score;
        
        [SyncVar(hook = nameof(OnTeam2ScoreChanged))]
        public int team2Score;
        public event Action IDInstalledEvent;

        public void OnInstallIID()
        {
            IDInstalledEvent?.Invoke();
        }
        
        [Server]
        public void AddScore(int team)
        {
            if (team == 0)
            {
                team1Score ++;
                if (team2Score > 0)
                    team2Score--;
            }
            else if (team == 1)
            {
                team2Score ++;
                if (team1Score > 0)
                    team1Score--;
            }
        }
        
        private void OnTeam1ScoreChanged(int oldValue, int newValue)
        {
            Debug.Log("OnTeam1Score");
            UpdateTeam0ScoreUI?.Invoke(newValue);
        }
        
        private void OnTeam2ScoreChanged(int oldValue, int newValue)
        {
            Debug.Log("OnTeam2Score");
            UpdateTeam1ScoreUI?.Invoke(newValue);
        }
        
        public int GetTeam1Score()
        {
            return team1Score;
        }

        public int GetTeam2Score()
        {
            return team2Score;
        }
    }
}