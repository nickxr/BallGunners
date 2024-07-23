using Network;
using VContainer;
using VContainer.Unity;

namespace Infrastructure
{
    public class RootLifetimeScope : LifetimeScope
    {
        public CustomNetworkManager networkManagerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            CustomNetworkManager networkManager = Instantiate(networkManagerPrefab);
            networkManager.dontDestroyOnLoad = true;
            builder.RegisterInstance(networkManager);
        }
    }
}