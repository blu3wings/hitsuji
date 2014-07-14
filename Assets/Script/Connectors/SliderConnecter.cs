using UnityEngine;
using System.Collections;

public class SliderConnecter : MonoBehaviour
{

    public UISliderName sliderName;

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

        _uiController.registerUISliders(sliderName, gameObject.GetComponent<UISlider>());
    }
}
