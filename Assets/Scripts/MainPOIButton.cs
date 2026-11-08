using UnityEngine;
using UnityEngine.UI;

public class MainPOIButton : MonoBehaviour
{
    Button thisButton;
    public Button otherButton1;
    public Button otherButton2;

    public Button subPOI1;
    public Button subPOI2;
    public Button subPOI3;
    public Button subPOI4;

    public Button backButton;

    public bool poi1accessable;
    public bool poi2accessable;
    public bool poi3accessable;
    public bool poi4accessable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisButton = this.GetComponent<Button>();
        thisButton.onClick.AddListener(selectMainPoi);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectMainPoi()
    {
        backButton.gameObject.SetActive(true);
        otherButton1.gameObject.SetActive(false);
        otherButton2.gameObject.SetActive(false);
        if (!poi1accessable)
        {
            subPOI1.gameObject.SetActive(true);
        }
        if (!poi2accessable)
        {
            subPOI2.gameObject.SetActive(true);
        }
        if (!poi3accessable)
        {
            subPOI3.gameObject.SetActive(true);
        }
        if (!poi4accessable)
        {
            subPOI4.gameObject.SetActive(true);
        }
        thisButton.gameObject.SetActive(false);
    }
}
