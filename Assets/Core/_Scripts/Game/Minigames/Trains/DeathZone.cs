using RapidPrototyping.TicTacMix.Trains;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix
{
    public class DeathZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                collision.gameObject.GetComponent<PlayerController>()._gameManager.PlayerDead(collision.gameObject.GetComponent<PlayerController>()._isPlayerO);
                collision.gameObject.GetComponent<PlayerController>().DestroyAll();
                print("destroyDEATHZONE");
            }

        }
    }
}
