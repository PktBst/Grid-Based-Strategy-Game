using UnityEngine;

namespace Cube
{
    public class CubeScript : MonoBehaviour
    {
        public bool Walkable = true;
        public Vector3 Pos;

        private void Start()
        {
            Pos = transform.position;
        }


    }
}
