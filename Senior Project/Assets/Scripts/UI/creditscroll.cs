using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class creditscroll : MonoBehaviour
{
    public float scroll_speed;

    private float initial_y_point;


    // Start is called before the first frame update
    public StartManager StartManager;
    public TextMeshProUGUI credit;
    public float changeRatio;
    void Start()
    {
        initial_y_point = credit.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (credit.transform.position.y < Screen.height * changeRatio)
        {
            credit.transform.Translate(0,scroll_speed,0);
        }
        else
        {
            StartManager.backButton.GetComponent<TextMeshProUGUI>().enabled = true;
            StartManager.backButton.GetComponent<Button>().interactable = true;
            StartManager.cursorImage.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(StartManager.curBackButton);
        }
    }

    public void Reset()
    {
        StartManager.backButton.GetComponent<TextMeshProUGUI>().enabled = false;
        StartManager.backButton.GetComponent<Button>().interactable = false;
        credit.transform.position = new Vector3(credit.transform.position.x, initial_y_point, credit.transform.position.y);
    }
}
