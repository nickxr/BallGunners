using Network;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class LobbyLifetimescope : LifetimeScope
    {
        [SerializeField] private LobbyUI lobbyUIPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("LobbyInstaller started");
            LobbyUI lobbyUI = Instantiate(lobbyUIPrefab);
            builder.RegisterComponent(lobbyUI);
            builder.Register<LobbyController>(Lifetime.Singleton);
            builder.RegisterBuildCallback((container) =>
            {
                container.Resolve<LobbyController>();
            });
        }
    }
}