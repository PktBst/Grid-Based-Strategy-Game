using UnityEngine;

[CreateAssetMenu(fileName = "NewGridData", menuName = "Grid/GridData")]
public class GridData : ScriptableObject
{
    public const int GridWidth = 10;
    public const int GridHeight = 10;

    [SerializeField] private bool[] grid = new bool[GridWidth * GridHeight]; 

    public bool GetValue(int x, int y)
    {
        if (IsValidPosition(x, y))
            return grid[y * GridWidth + x];
        return false;
    }
    public bool ToggleValue(int x, int y)
    {
        if (IsValidPosition(x, y))
            grid[y * GridWidth + x] = !grid[y * GridWidth + x];
        return grid[y * GridWidth + x];
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < GridWidth && y >= 0 && y < GridHeight;
    }
    //public void PrintGrid()
    //{
    //    for (int y = 0; y < GridHeight; y++)
    //    {
    //        string row = "";
    //        for (int x = 0; x < GridWidth; x++)
    //        {
    //            row += GetValue(x, y) ? "1 " : "0 "; 
    //        }
    //        Debug.Log(row); 
    //    }
    //}
}
