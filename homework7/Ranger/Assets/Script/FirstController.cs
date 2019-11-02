using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction
{
    Dictionary<int, GameObject> ranger_dict = null;
    RangerFactory ranger_factory;
    GameObject player = null;
    Action action = null;
    int score = 0;
    int player_vision = 4;
    bool game_state = false;

    //  awake
    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        ranger_factory = RangerFactory.ranger_factory;
        if(action == null) 
        {
            action = gameObject.AddComponent<Action>();
        }
        if (player == null && ranger_dict == null)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Map"), new Vector3(0, 0, 0), Quaternion.identity);
            player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            ranger_dict = ranger_factory.GetRanger();
        }
        if (player.GetComponent<Rigidbody>())
        {
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }
        LoadResources();
    }

    //  load
    public void LoadResources()
    {
        //
    }

    //  update
    void Update ()
    {
        if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
        {
            player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
        }
        if (player.transform.position.y <= 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public bool GetGameState()
    {
        return game_state;
    }

    void OnEnable()
    {
        EventManager.score_temp += AddScore;
        EventManager.gameover_temp += GameOver;
    }

    void OnDisable()
    {
        EventManager.score_temp -= AddScore;
        EventManager.gameover_temp -= GameOver;
    }

    void AddScore()
    {
        if (game_state)
        {
            ++score;
            ranger_dict[player_vision].GetComponent<Ranger>().isTracerting = true;
            action.Tracert(ranger_dict[player_vision], player);
        }
    }

    void GameOver()
    {
        action.AllFinished();
        ranger_dict[player_vision].GetComponent<Ranger>().isTracerting = false;
        game_state = false;
    }

    public void PlayerMove(float move_x, float move_z)
    {
        if (game_state && player != null)
        {
            player.transform.Translate(0, 0, move_z * 4f * Time.deltaTime);
            player.transform.Rotate(0, move_x * 50f * Time.deltaTime, 0);
        }
    }

    public void SetPlayerArea(int x)
    {
        // Debug.Log(player_vision);
        // Debug.Log(x);
        // Debug.Log(game_state);
        // Debug.Log("-------");
        if (player_vision != x && game_state)
        {
            ranger_dict[player_vision].GetComponent<Ranger>().isTracerting = false;
            player_vision = x;
        }
    }

    public void Restart()
    {
        ranger_factory.PatrolFinished();
        score = 0;
        game_state = true;
        player.transform.position = new Vector3(0, 0, 0);
        ranger_dict[player_vision].GetComponent<Ranger>().isTracerting = true;
        action.Tracert(ranger_dict[player_vision], player);
        foreach (GameObject x in ranger_dict.Values)
        {
            if (!x.GetComponent<Ranger>().isTracerting)
            {
                action.Patrol(x);
            }
        }
    }
}