using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiscActions : MonoBehaviour
{
    public GameObject StatusBoard;
    public TextMeshPro LogDisplay;
    Status.Importance LogLevel = Status.Importance.SomewhatImportant;
    public void ToggleStatusBoard()
    {
        StatusBoard.SetActive(!StatusBoard.activeInHierarchy);
    }
    public void ChangeLogLevel()
    {
        LogLevel++;
        LogLevel = (Status.Importance)((int)LogLevel % 6);
        switch (LogLevel)
        {
            case Status.Importance.Critical:
                LogDisplay.text = "Critical";
                break;
            case Status.Importance.Important:
                LogDisplay.text = "Important";
                break;
            case Status.Importance.NotImportant:
                LogDisplay.text = "Not Important";
                break;
            case Status.Importance.SlightlyImportant:
                LogDisplay.text = "Slightly Important";
                break;
            case Status.Importance.SomewhatImportant:
                LogDisplay.text = "Somewhat Important";
                break;
            case Status.Importance.VeryImportant:
                LogDisplay.text = "Very Important";
                break;
            default:
                LogDisplay.text = LogLevel.ToString();
                break;

        }
        Status.ChangeLogLevel(LogLevel);
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeLogLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
