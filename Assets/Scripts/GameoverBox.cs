using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverBox : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Beaker")
        {
            GameManager.instance.GameOver();
            print("gameover in Box!");
        }
    }
}
