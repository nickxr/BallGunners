using Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class PlayerInstaller : LifetimeScope
    {
        [SerializeField] private GoalController goalController;
        [SerializeField] private PlayerController playerController;

        protected override void Configure(IContainerBuilder builder)
        {
            playerController ??= GetComponent<PlayerController>();
            goalController ??= GetComponentInChildren<GoalController>();
            
            builder.RegisterComponent(goalController);
            builder.RegisterComponent(playerController);
        }
    }
}