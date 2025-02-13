using Cube;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerMovementScript : MonoBehaviour
    {
        public Vector3 PlayerCurrentPos;
        public Vector3 SelectedTilePos;
        public GridData gridData;
        [SerializeField] TextMeshProUGUI PlayerPosLabel;
        [SerializeField] TextMeshProUGUI SelectedTileLabel;

        private bool isMoving = false;

        void Update()
        {
            PlayerCurrentPos = transform.position;
            PlayerPosLabel.text = "Player Pos : ("+ PlayerCurrentPos.x+","+PlayerCurrentPos.z+")";
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                ReadTileOnClick();
            }
        }

        private void ReadTileOnClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    SelectedTilePos = cubeScript.Pos;
                    SelectedTileLabel.text = "Selected Tile : (" + SelectedTilePos.x + "," + SelectedTilePos.z + ")";
                    Debug.Log("Clicked Tile Selected");

                    CalculateAndMovePlayerToSelectedTile();
                }
            }
        }

        void CalculateAndMovePlayerToSelectedTile()
        {
            isMoving = true;
            //player movement
            isMoving = false;
        }
    }
}
