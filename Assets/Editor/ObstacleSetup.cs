using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ObstacleSetup : EditorWindow
{
    static ObstacleSetup window;
    private GridData gridData;

    //make menu item 
    [MenuItem("MyTools/Obstacle Setup")] 
    public static void InitWindow()
    {
        window = GetWindow<ObstacleSetup>("Obstacle Setup");
        window.Show();
    }

    //load gridData
    private void OnEnable()
    {
        gridData = AssetDatabase.LoadAssetAtPath<GridData>("Assets/Scripts/NewGridData.asset");
    }

    //GUI 
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Obstacle Setup", EditorStyles.boldLabel);
        GUILayout.Space(10); 

        int gridSize = GridData.GridWidth;
        for (int col = 0; col < gridSize; col++)
        {
            GUILayout.BeginHorizontal(); 
            for (int row = 0; row < gridSize; row++)
            {
                int buttonIndex = row * gridSize + col + 1;
                bool isToggled = gridData.GetValue(col, row); //toogle values
                GUI.backgroundColor = isToggled ? Color.red : Color.green; //toogle colors

                if (GUILayout.Button(col + "," + row, GUILayout.Width(30), GUILayout.Height(30)))//setting height and width of buttons
                {
                    bool newState = gridData.ToggleValue(col, row); 
                    EditorUtility.SetDirty(gridData); //saving changed data
                }
            }
            //end line after a row
            GUILayout.EndHorizontal(); 
        }      
        GUI.backgroundColor = Color.white;
    }
}
