using Network;
using Player;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class LobbyLifetimescope : LifetimeScope
    {
        [SerializeField] private LobbyController lobbyController;
        [SerializeField] private LobbyUI lobbyUIPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("LobbyInstaller started");
            LobbyUI lobbyUI = Instantiate(lobbyUIPrefab);
            builder.RegisterComponent(lobbyUI);
            builder.RegisterComponent(lobbyController);
            builder.RegisterBuildCallback((container) =>
            {
                container.Resolve<CustomNetworkManager>().Initialize(lobbyController);
            });
        }
    }
}