using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using UnityEngine;

public class Relay : MonoBehaviour
{
    private static readonly object lockObject = new object();
    private static Relay instance;

    public static Relay Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                    instance = new Relay();
                return instance;
            }
        }
    }

    private Relay() { }

    public async Task<string>? CreateRelay(int numberOfPlayers)
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var allocation = await RelayService.Instance.CreateAllocationAsync(numberOfPlayers-1);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch { }

        return null;
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch { }
    }
}
