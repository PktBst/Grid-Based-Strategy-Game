using Cube;
using UnityEngine;

namespace GridMaker
{
    public class GridMaker : MonoBehaviour
    {
        [SerializeField] GameObject cubePrefab;
        [SerializeField] int gridSize = 10;

        void Start()
        {
            GenerateGrid();
        }

        void GenerateGrid()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    GameObject tile = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    tile.transform.SetParent(transform);
                    tile.name = "Tile (" + x + "," + y + ")";
                }
            }
        }
    }
}

