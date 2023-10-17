using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearZone : MonoBehaviour
{
    public int gameClearScore = 6;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(GameManager.instance != null)
            {
                if(GameManager.instance.score >= gameClearScore)
                {
                    GameManager.instance.ClearGame();
                }
            }
        }
    }
}
