using Cube;
using TMPro;
using UnityEngine;

namespace GridMaker
{
    public class TileHoverReader : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CoordinatesLabel;
        //[SerializeField] TextMeshProUGUI WalkableLabel;
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    CoordinatesLabel.text = "Coordinates : (" + cubeScript.Pos.x.ToString() +","+ cubeScript.Pos.z.ToString()+")";
                    //WalkableLabel.text = "Walkable : " + cubeScript.Walkable.ToString();
                    //Debug.Log("Hit object contains CubeScript.");
                }
            }
        }
    }
}
