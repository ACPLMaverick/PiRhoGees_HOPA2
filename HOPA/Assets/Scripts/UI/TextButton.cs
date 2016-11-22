using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextButton : MonoBehaviour {

    public string Title;
    public string TextPath;
    public TextReadingPanel ReadingPanel;
    public Button BackButton;

    protected Button _myButton;
    protected Vector2 _panelInitialPosition;

    protected virtual void Awake()
    {
        _myButton = GetComponent<Button>();
    }

	// Use this for initialization
	void Start () {
        _myButton.onClick.AddListener(OpenText);
        _panelInitialPosition = ReadingPanel.transform.localPosition;

        _myButton.GetComponentInChildren<Text>().text = Title;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenText()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(CloseText);

        ReadingPanel.GetComponent<Switchable>().SwitchOn();
        ReadingPanel.TextSlider.GetComponent<Switchable>().SwitchOn();
        ReadingPanel.Title.text = Title;
        ReadingPanel.Content.text = "New Text";
        if (TextPath != "")
        {
            ReadingPanel.Content.text = Resources.Load<TextAsset>(TextPath).text;
        }

        transform.parent.GetComponent<Switchable>().SwitchOff();

        StartCoroutine(SetReadingSpace());
    }

    public void CloseText()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() => SceneChangeManager.Instance.ChangeScene(0));
        ReadingPanel.TextSlider.GetComponent<Switchable>().SwitchOff();
        ReadingPanel.GetComponent<Switchable>().SwitchOff();
        ReadingPanel.transform.localPosition = _panelInitialPosition;
        ReadingPanel.TextSlider.value = 0;

        transform.parent.GetComponent<Switchable>().SwitchOn();
    }

    IEnumerator SetReadingSpace()
    {
        // Wait for text to size itself
        yield return new WaitForEndOfFrame();
        ReadingPanel.SetReadingSpace();
        ReadingPanel.SetSliderSize();
    }
}
