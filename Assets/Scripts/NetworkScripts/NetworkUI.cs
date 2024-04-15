using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField]
    private Button ServerButton;
    [SerializeField]
    private Button HostButton;
    [SerializeField]
    private Button ClientButton;
    [SerializeField]
    private TMP_InputField IpAddressInputfield;
    [SerializeField]
    private TMP_InputField PortInputfield;
    [SerializeField]
    private Button HostRelayButton;
    [SerializeField]
    private Button JoinRelayButton;
    [SerializeField]
    private TMP_InputField RelayCodeInputfield;

    private bool IsMenuActive = true;
    private string RelayJoinCode = "";

    private void Start()
    {
        ServerButton.onClick.AddListener(() =>
        {
            SetupConnection();
            NetworkManager.Singleton.StartServer();
        });
        HostButton.onClick.AddListener(() =>
        {
            SetupConnection();
            NetworkManager.Singleton.StartHost();
        });
        ClientButton.onClick.AddListener(() =>
        {
            SetupConnection();
            NetworkManager.Singleton.StartClient();
        });
        HostRelayButton.onClick.AddListener(() =>
        {
            Relay.Instance.CreateRelay(4).ContinueWith((t) => { lock (RelayJoinCode) { RelayJoinCode = t.Result ?? ""; } });
        });
        JoinRelayButton.onClick.AddListener(() =>
        {
            Relay.Instance.JoinRelay(RelayCodeInputfield.text);
        });
    }

    private void SetupConnection()
    {
        var transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = IpAddressInputfield.text == "" ? "127.0.0.1" : IpAddressInputfield.text;
        transport.ConnectionData.Port = Convert.ToUInt16(PortInputfield.text == "" ? "7777" : PortInputfield.text);
    }

    private void Update()
    {
        lock (RelayJoinCode) { if (RelayJoinCode != "") RelayCodeInputfield.text = RelayJoinCode; }
        if (Input.GetKeyDown(KeyCode.N))
        {
            IsMenuActive = !IsMenuActive;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, IsMenuActive ? 0 : 10000, 0);
        }
    }
}
