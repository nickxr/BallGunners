using Player;
using Scores;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class PlayerInstaller : LifetimeScope
    {
        [SerializeField] private GoalController goalController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(goalController);
        }
    }
}