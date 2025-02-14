using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ObstacleButtonUIGenerator : MonoBehaviour
{
    public int gridSize = 10; 
    public GameObject buttonPrefab;  
    public float buttonSpacing = 10f; 
    public RectTransform gridPanel;
    public GridData gridData;
    [SerializeField]private Color selectedButtonColor;
    [SerializeField]private Color unselectedButtonColor;

    void Start()
    {
        GenerateGrid(gridSize);
    }

    void GenerateGrid(int size)
    {

        // get button width and height
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().sizeDelta.x;
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;

        // to calculate grid size
        float gridX = size * (buttonWidth + buttonSpacing);
        float gridY = size * (buttonHeight + buttonSpacing);
        gridPanel.sizeDelta = new Vector2(gridX, gridY);

        // putting buttons in a grid layout
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                CreateButton(row, col, buttonWidth, buttonHeight);
            }
        }
    }

    void CreateButton(int row, int col, float buttonWidth, float buttonHeight)
    {
        Button newButton = Instantiate(buttonPrefab, gridPanel).GetComponent<Button>();
        SetCurrentButtonState(row, col, newButton);
        newButton.onClick.AddListener(() => ToggleButtonValue(row,col,newButton));

        // set the position of the button in grid
        RectTransform buttonRect = newButton.GetComponent<RectTransform>();
        float ButtonX = col * (buttonWidth + buttonSpacing);
        float ButtonY = -row * (buttonHeight + buttonSpacing);
        buttonRect.localPosition = new Vector2(ButtonX, ButtonY);

        newButton.name = "Button (" + row + "," + col + ")";
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = row + "," + col;
    }

    void ToggleButtonValue(int row, int col,Button button)
    {
        bool value = gridData.ToggleValue(row, col);
        if (value)
        {
            button.GetComponent<Image>().color = selectedButtonColor;
        }
        else
        {
            button.GetComponent<Image>().color = unselectedButtonColor;
        }

    }

    void SetCurrentButtonState(int row, int col,Button button) {
        bool currentButtonState = gridData.GetValue(row,col);
        if (currentButtonState)
        {
            button.GetComponent<Image>().color = selectedButtonColor;
        }
        else 
        {
            button.GetComponent<Image>().color = unselectedButtonColor;
        }
    }
}
