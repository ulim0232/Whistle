using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(GameManager.instance != null)
            {
                if(GameManager.instance.score >= 4 )
                {
                    GameManager.instance.ClearGame();
                }
            }
        }
    }
}
