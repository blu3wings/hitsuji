using UnityEngine;
using System.Collections;

public class ButtonConnector : MonoBehaviour 
{
    public UIAction uiAction;

    private UIController _uiController;

    private void Start()
    {
        //Lets make sure the GameController has been initialized
        if (GameObject.FindGameObjectWithTag("GameController")
           .GetComponent<GameController>())
        {
            _uiController = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameController>().uiController;
        }
    }

    private void OnPress(bool isPressed)
    {
        if(isPressed)
        {
            if (_uiController != null && uiAction != null)
            {
                _uiController.onButtonClick(uiAction);
            }
        }        
    }
}