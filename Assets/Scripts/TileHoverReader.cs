using Cube;
using TMPro;
using UnityEngine;

namespace GridMaker
{
    public class TileHoverReader : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CoordinatesLabel;
        private Transform lastHoveredInner; 
        private Color lastHoveredOriginalColor; 

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CubeScript cubeScript = hit.collider.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    CoordinatesLabel.text = "Coordinates : (" + cubeScript.Pos.x.ToString() + "," + cubeScript.Pos.z.ToString() + ")";

                    //change color on hover
                    Transform innerChild = hit.collider.transform.Find("outer");
                    if (innerChild != null)
                    {
                        Renderer innerRenderer = innerChild.GetComponent<Renderer>();

                        if (innerRenderer != null)
                        {
                            // If hovering over a new cube, reset the previous one first
                            if (lastHoveredInner != innerChild)
                            {
                                ResetLastHovered();

                                // Store the new hovered cube info
                                lastHoveredInner = innerChild;
                                lastHoveredOriginalColor = innerRenderer.material.color;
                                innerRenderer.material.color = Color.blue;
                            }
                        }
                    }
                }
            }
            else
            {
                ResetLastHovered();
            }
        }

        private void ResetLastHovered()
        {
            if (lastHoveredInner != null)
            {
                Renderer innerRenderer = lastHoveredInner.GetComponent<Renderer>();
                if (innerRenderer != null)
                {
                    innerRenderer.material.color = lastHoveredOriginalColor;
                }
                lastHoveredInner = null; 
            }
        }
    }
}
