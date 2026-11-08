using UnityEngine;
using UnityEngine.UI;

public class SubPOIButton : MonoBehaviour
{
    public int MainPOIIndex;
    public int SubPOIIndex;

    public string sceneName;

    public Button mainPOI1;
    public Button mainPOI2;
    public Button mainPOI3;

    public Button backButton;

    Button thisSubPOI;
    public Button subPOI1;
    public Button subPOI2;
    public Button subPOI3;



    void Start()
    {
        thisSubPOI = this.GetComponent<Button>();
        thisSubPOI.onClick.AddListener(selectThisSubPOI);
        backButton.onClick.AddListener(goBack);
    }

    public void goBack()
    {
        mainPOI1.gameObject.SetActive(true);
        mainPOI2.gameObject.SetActive(true);
        mainPOI3.gameObject.SetActive(true);
        subPOI1.gameObject.SetActive(false);
        subPOI2.gameObject.SetActive(false);
        subPOI3.gameObject.SetActive(false);
        thisSubPOI.gameObject.SetActive(false);
    }

    public void selectThisSubPOI()
    {
        GameManager.Instance.changeScene(MainPOIIndex, SubPOIIndex, sceneName);
    }


}