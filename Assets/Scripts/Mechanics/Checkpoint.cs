using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class Checkpoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            BoxCollider2D trigger = transform.gameObject.GetComponent<BoxCollider2D>();
            PlayerController player = other.transform.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetCheckpoint(transform);
                Destroy(trigger);
            }
        }
    }
}

