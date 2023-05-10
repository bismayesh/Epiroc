using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks {

    public GameObject playerPrefab;
    public List<Transform> playerSpawnLocations;
    
    private void Start() {
        Debug.Log("Connecting to master server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("<color=green><b>Connected!</b></color>");
        Debug.Log("Joining lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log("<color=green><b>Joined lobby!</b></color>");
        if (PhotonNetwork.CountOfRooms == 0) {
            Debug.Log("Couldn't find an active room... Creating a new one...");
            PhotonNetwork.CreateRoom("dev");
        }
        else {
            Debug.Log("Joining room...");
            PhotonNetwork.JoinRoom("dev");
        }
    }

    public override void OnCreatedRoom() {
        Debug.Log("<color=green><b>Successfully created a room!</b></color>");
    }
    
    public override void OnJoinedRoom() {
        if (PhotonNetwork.IsMasterClient) {
            SpawnPlayers();
        }
        else {
            Debug.Log("<color=green><b>Successfully joined a room!</b></color>");
            SpawnPlayers();
        }
    }

    public void SpawnPlayers() {
        var playerSpawnLocationIndex = (int)Random.Range(0, playerSpawnLocations.Count - 1);

        Debug.Log($"Spawning on <color=yellow>{playerSpawnLocations[playerSpawnLocationIndex].position}</color>");
        PhotonNetwork.Instantiate(
            playerPrefab.name, 
            playerSpawnLocations[playerSpawnLocationIndex].position, 
            Quaternion.identity
            );
    }
    
}

