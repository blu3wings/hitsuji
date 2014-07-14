using UnityEngine;
using System.Collections;

public class LabelConnector : MonoBehaviour 
{
    public UILabelName labelName;

    private UIController _uiController;

	private void Start () 
    {
        //Lets make sure the GameController has been initialized
        if (GameObject.FindGameObjectWithTag("GameController")
           .GetComponent<GameController>())
        {
            _uiController = GameObject.FindGameObjectWithTag("GameController")
                .GetComponent<GameController>().uiController;
        }

        _uiController.registerUILabels(labelName, gameObject.GetComponent<UILabel>());
	}
}