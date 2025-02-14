using UnityEngine;

[CreateAssetMenu(fileName = "NewGridData", menuName = "Grid/GridData")]
public class GridData : ScriptableObject
{
    public const int GridWidth = 10;
    public const int GridHeight = 10;

    // 100 long bool array, caused 2d array arent serializable
    [SerializeField] private bool[] grid = new bool[GridWidth * GridHeight]; 

    //getter
    public bool GetValue(int x, int y)
    {
        if (IsValidPosition(x, y))
            return grid[y * GridWidth + x];
        return false;
    }

    //toggle setter
    public bool ToggleValue(int x, int y)
    {
        if (IsValidPosition(x, y))
            grid[y * GridWidth + x] = !grid[y * GridWidth + x];
        return grid[y * GridWidth + x];
    }

    //checking if accessing the grid is availble or not
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < GridWidth && y >= 0 && y < GridHeight;
    }
}
