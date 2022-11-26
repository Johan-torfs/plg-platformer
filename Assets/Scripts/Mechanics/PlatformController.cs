using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class PlatformController : MonoBehaviour
    {
        public PatrolPath path;
        public float maxSpeed = 2.0f;
        internal PatrolPath.Mover mover;

        Vector2 move;

        // Update is called once per frame
        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(maxSpeed * 0.5f);
                move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
                transform.Translate(move);
            }
        }
    }
}
