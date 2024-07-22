using Scores;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ScoreUI scoreUI;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ScoreManager>(Lifetime.Singleton);
            builder.RegisterComponent(scoreUI);
        }
    }
}