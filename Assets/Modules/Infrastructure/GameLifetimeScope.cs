using Scores;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ScoreUI scoreUI;
        [SerializeField] private ServerScore serverScore;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(serverScore);
            builder.RegisterComponent(scoreUI);
        }
    }
}