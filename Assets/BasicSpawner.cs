using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicSpawner : MonoBehaviour , INetworkRunnerCallbacks
{
    public Team MyTeam;

    public static BasicSpawner Instance;

    public NetworkRunner _runner;
    [SerializeField] private NetworkPrefabRef red_playerPrefab;
    [SerializeField] private NetworkPrefabRef blue_playerPrefab;
    public Dictionary<string, List<NetworkObject>> _spawnedCharacters = new Dictionary<string, List<NetworkObject>>();

    [SerializeField]
    Toggle Red;

    [SerializeField]
    Toggle Blue;


    [SerializeField]
    Transform RedTransform;

    [SerializeField]
    Transform BlueTransform;

    [SerializeField]
    TMP_Text ConfirmTeamButton;

    [SerializeField]
    GameObject SelectTeam;

    [SerializeField]
    GameObject SearchingOpponent;

    [SerializeField]
    GameObject OpponentFound;

    [SerializeField]
    TMP_Text OpponentFoundText;

    [SerializeField]
    TMP_Text SearchingOpponentText;

    public string OtherPlayerId = "";

    [SerializeField]
    GameObject GameView;

    bool Attack1 = false;
    bool Attack2 = false;
    public enum Team
    {
        Red,Blue
    }

    public NetworkObject PickOpponent()
    {
        if (_spawnedCharacters[OtherPlayerId].Count == 0) return null;
        return _spawnedCharacters[OtherPlayerId][UnityEngine.Random.Range(0, _spawnedCharacters.Count)];
    }

    private void Awake()
    {
        Instance = this;
    }
    private bool PlayerJoined = false;

    // Start is called before the first frame update
    void Start()
    {
        Red.onValueChanged.AddListener(SetTeam);
        Blue.onValueChanged.AddListener(SetTeam);
    }

    public void SetTeam(bool team)
    {


        if (Red.isOn)
        {
            ConfirmTeamButton.transform.parent.gameObject.SetActive(true);
            MyTeam = Team.Red;
            ConfirmTeamButton.text = "Confirm Red";
        }
        else if (Blue.isOn)
        {
            ConfirmTeamButton.transform.parent.gameObject.SetActive(true);
            MyTeam = Team.Blue;
            ConfirmTeamButton.text = "Confirm Blue";
        }
        else ConfirmTeamButton.transform.parent.gameObject.SetActive(false);

    }

    public void LoadGame()
    {
        StartCoroutine(StartGameVisual());
    }


    IEnumerator StartGameVisual()
    {

        SelectTeam.SetActive(false);
        SearchingOpponent.SetActive(true);

        if (MyTeam == Team.Red)
        {
            StartGame(GameMode.Host);
            SearchingOpponentText.text = "Waiting for Blue Opponent";
        }
        else
        {
           
            StartGame(GameMode.Client);
            SearchingOpponentText.text = "Waiting for Red Opponent";

        }
        yield return new WaitUntil(() => _runner !=  null);
        yield return new WaitUntil(() => _runner.SessionInfo.PlayerCount == 2);
        SearchingOpponent.SetActive(false);
        OpponentFound.SetActive(true);
        int waittime = 3;
        while (waittime > 0)
        {
            OpponentFoundText.text = "Match Found Starting in " + waittime + " ...";
            waittime--;
            yield return new WaitForSeconds(1f);
        }
        OpponentFound.SetActive(false);
        GameView.SetActive(true);
      

    }

    public void Attack01()
    {
        //if(MyTeam == Team.Red)
        //{
        //    var list = GameObject.FindGameObjectsWithTag("RedPlayer");
        //    foreach(var g in list)
        //    {
        //        g.GetComponent<Player>().Attack01();
        //    }
        //}
        //else
        //{

        //    var list = GameObject.FindGameObjectsWithTag("BluePlayer");
        //    foreach (var g in list)
        //    {
        //        g.GetComponent<Player>().Attack01();
        //    }

        //}
        Attack1 = true;
    }

    public void Attack02()
    {
        //if (MyTeam == Team.Red)
        //{
        //    var list = GameObject.FindGameObjectsWithTag("RedPlayer");
        //    foreach (var g in list)
        //    {
        //        g.GetComponent<Player>().Attack02();
        //    }
        //}
        //else
        //{

        //    var list = GameObject.FindGameObjectsWithTag("BluePlayer");
        //    foreach (var g in list)
        //    {
        //        g.GetComponent<Player>().Attack02();
        //    }

        //}
        Attack2 = true;

    }


    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Attack1) data.hasAttacked1 = true;
        if (Attack2) data.hasAttacked2 = true;
        Attack1 = false;
        Attack2 = false;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        if (runner.IsServer)
        {
            
            // Create a unique position for the player
            if (_runner.LocalPlayer == player)
            {
                if(MyTeam == Team.Red)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        NetworkObject networkPlayerObject = runner.Spawn(red_playerPrefab, RedTransform.GetChild(i).position, RedTransform.GetChild(i).rotation, player);
                        if (_spawnedCharacters.ContainsKey(player.PlayerId.ToString()) == false)
                        {
                            _spawnedCharacters.Add(player.PlayerId.ToString(), new List<NetworkObject>());
                        }
                        _spawnedCharacters[player.PlayerId.ToString()].Add(networkPlayerObject);
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        NetworkObject networkPlayerObject = runner.Spawn(blue_playerPrefab, BlueTransform.GetChild(i).position, BlueTransform.GetChild(i).rotation, player);
                        if (_spawnedCharacters.ContainsKey(player.PlayerId.ToString()) == false)
                        {
                            _spawnedCharacters.Add(player.PlayerId.ToString(), new List<NetworkObject>());
                        }
                        _spawnedCharacters[player.PlayerId.ToString()].Add(networkPlayerObject);
                    }
                }

            }
            else
            {
                OtherPlayerId = player.PlayerId.ToString();
                if (MyTeam == Team.Blue)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        NetworkObject networkPlayerObject = runner.Spawn(red_playerPrefab, RedTransform.GetChild(i).position, RedTransform.GetChild(i).rotation, player);
                        if (_spawnedCharacters.ContainsKey(player.PlayerId.ToString()) == false)
                        {
                            _spawnedCharacters.Add(player.PlayerId.ToString(), new List<NetworkObject>());
                        }
                        _spawnedCharacters[player.PlayerId.ToString()].Add(networkPlayerObject);
                    }
                }
                else
                {

                    for(int i = 0;i < 5;i++)
                    {
                        NetworkObject networkPlayerObject = runner.Spawn(blue_playerPrefab, BlueTransform.GetChild(i).position, BlueTransform.GetChild(i).rotation, player);
                        if (_spawnedCharacters.ContainsKey(player.PlayerId.ToString()) == false)
                        {
                            _spawnedCharacters.Add(player.PlayerId.ToString(), new List<NetworkObject>());
                        }
                        _spawnedCharacters[player.PlayerId.ToString()].Add(networkPlayerObject);
                    }
         

                }
            }

        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        if (_spawnedCharacters.TryGetValue(player.PlayerId.ToString(), out List<NetworkObject> networkObject))
        {
            foreach(var e in networkObject)
            runner.Despawn(e);
            _spawnedCharacters.Remove(player.PlayerId.ToString());
        }

    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    // Red team Hosts the Game , Blue Team joins the Game

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {      
            GameMode = mode,
            PlayerCount = 2, // Max Player set to 2
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }


 
}
