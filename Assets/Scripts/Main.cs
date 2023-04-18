using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using System;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;


public class Main : NetworkBehaviour
{
    public It4080.NetworkSettings netSettings;
    private Button btnStart;


    //-------------------------
    // Start

    void Start()
    {
        // Network Start
        netSettings.startServer += NetSettingsOnServerStart;
        netSettings.startHost += NetSettingsOnHostStart;
        netSettings.startClient += NetSettingsOnClientStart;

        // Button Start Game
        btnStart = GameObject.Find("BtnStartGame").GetComponent<Button>();
        btnStart.onClick.AddListener(BtnStartGameOnClick);
    }

    void Update()
    {

    }

    // Button Start Game
    private void BtnStartGameOnClick()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Started Game");
        NetworkManager.SceneManager.LoadScene("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    // Start Client
    private void StartClient(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();
        //netSettings.hide();
        Debug.Log("started client");
    }

    // Start Host
    private void StartHost(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;


        NetworkManager.Singleton.StartHost();
        //netSettings.hide();
        Debug.Log("started host");
    }

    // Start Server
    private void StartServer(IPAddress ip, ushort port)
    {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;


        NetworkManager.Singleton.StartServer();
        //netSettings.hide();
        Debug.Log("started server");
    }


    //-----------------------------
    // Events

    private void HostOnClientConnected(ulong clientID)
    {
        Debug.Log($"client connected to me: {clientID}");
    }

    private void HostOnClientDisconnected(ulong clientID)
    {
        Debug.Log($"client disconnected from me: {clientID}");
    }

    private void ClientOnClientConnected(ulong clientID)
    {

    }

    private void ClientOnClientDisconnected(ulong clientID)
    {

    }

    private void NetSettingsOnServerStart(IPAddress ip, ushort port)
    {
        StartServer(ip, port);
    }

    private void NetSettingsOnHostStart(IPAddress ip, ushort port)
    {
        StartHost(ip, port);
    }

    private void NetSettingsOnClientStart(IPAddress ip, ushort port)
    {
        StartClient(ip, port);
    }
}
