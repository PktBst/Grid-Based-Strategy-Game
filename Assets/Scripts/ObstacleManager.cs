using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject Obstacle;
    [SerializeField] int gridSize = 10;
    [SerializeField] GridData gridData;

    void Start()
    {
        GenerateObstacleGrid();
    }
    void GenerateObstacleGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {

                if (gridData.GetValue(x,y)) 
                {
                    GameObject tile = Instantiate(Obstacle, new Vector3(x, 1, y), Quaternion.identity);
                    tile.transform.SetParent(transform);
                    tile.name = "Obstacle (" + x + "," + y + ")";
                }
                
                
            }
        }
    }
}
