﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextButton : MonoBehaviour {

    public string Title;
    public string TextPath;
    public GameObject ReadingPanel;
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

        ReadingPanel.GetComponent<TextReadingPanel>().Title.text = Title;
        if (TextPath != "")
        {
            ReadingPanel.GetComponent<TextReadingPanel>().Content.text = Resources.Load<TextAsset>(TextPath).text;
        }

        ReadingPanel.GetComponent<Switchable>().SwitchOn();
        transform.parent.GetComponent<Switchable>().SwitchOff();
    }

    public void CloseText()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() => SceneChangeManager.Instance.ChangeScene(0));
        ReadingPanel.GetComponent<Switchable>().SwitchOff();
        ReadingPanel.transform.localPosition = _panelInitialPosition;

        transform.parent.GetComponent<Switchable>().SwitchOn();
    }
}