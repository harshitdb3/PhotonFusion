using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    const float TotalHealth = 100;
    public float currentHealth = 100;
    public static GameObject[] redPlayers;
    public static GameObject[] bluePlayers;
    [SerializeField]
    List<Image> fillImagered;
    [SerializeField]
    List<Image> fillImageblue;

    //public GameObject WinPanel;
    //public TMP_Text winText;
    // Start is called before the first frame update
    void Awake()
    {
            
            Instance = this;       
    }

    //public void UpdateWinUI()
    //{
    //    bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
    //    redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
    //    if (bluePlayers[0].GetComponent<Player>().Health == 0 )
    //    {
    //        UIManager.Instance.WinPanel.SetActive(true);
    //        if (BasicSpawner.Instance.MyTeam == BasicSpawner.Team.Red)
    //        {
    //            UIManager.Instance.winText.text = "Blue Won";
    //        }
    //        else
    //        {
    //            UIManager.Instance.winText.text = "Red Won";
    //        }
    //    }
    //    if (redPlayers[0].GetComponent<Player>().Health == 0)
    //    {
    //        UIManager.Instance.WinPanel.SetActive(true);
    //        if (BasicSpawner.Instance.MyTeam == BasicSpawner.Team.Red)
    //        {
    //            UIManager.Instance.winText.text = "Blue Won";
    //        }
    //        else
    //        {
    //            UIManager.Instance.winText.text = "Red Won";
    //        }
    //    }
    //}

    void Update()
    {
        bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
        redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
        if (redPlayers == null || bluePlayers == null) return;


        for (int i = 0;i < redPlayers.Length;i++)
        {
            if (redPlayers[i].GetComponent<Player>().isSpawned)
            {
                 fillImagered[i].fillAmount = redPlayers[i].GetComponent<Player>().Health / TotalHealth;
                 
            }

        }

        for (int i = 0; i < bluePlayers.Length; i++)
        {
         
            if (bluePlayers[i].GetComponent<Player>().isSpawned)
            {
               
                fillImageblue[i].fillAmount = bluePlayers[i].GetComponent<Player>().Health / TotalHealth;
               
            }
        }



    }
}
