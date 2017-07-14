using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipmentManager : Singleton<EquipmentManager>
{
    #region enums

    #endregion

    #region constants
    #endregion

    #region public

    public RectTransform PanelPickableList;
    public Button ButtonBack;
    public LanguageManager LangManager;

    #endregion

    #region properties

    #endregion

    #region private

    private List<PickableObject> _pickableList;

    private Dictionary<PickableObject, Text> _allPickablesDict;

    #endregion

    #region functions

    // Use this for initialization
    void Start ()
    {
        _allPickablesDict = new Dictionary<PickableObject, Text>();
        _pickableList = new List<PickableObject>();
        PanelPickableList.gameObject.SetActive(true);

        StartGUIPickables();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AddObjectToList(PickableObject obj, float lagSeconds)
    {
        StartCoroutine(AddObjectToListCoroutine(obj, lagSeconds));
    }

    public void FlushOnNextRoom()
    {
        _allPickablesDict.Clear();
        StartGUIPickables();
    }

    public void DisplayMapButton(bool display, bool interactable)
    {
        ButtonBack.gameObject.SetActive(display);
        ButtonBack.interactable = interactable;
    }

    public void ChangeTextPickedStatus(Text text, Button button, bool status)
    {
        text.fontStyle = !status ? FontStyle.Bold : FontStyle.BoldAndItalic;
        text.color = !status ? Color.white : new Color(0.8f, 0.8f, 0.8f);
        //button.interactable = !status;
    }

    private IEnumerator AddObjectToListCoroutine(PickableObject obj, float lagSeconds)
    {
        yield return new WaitForSeconds(lagSeconds);

        _pickableList.Add(obj);

        yield return null;
    }

    private void StartGUIPickables()
    {
        List<PickableObject> pickablesOnLevel = GameManager.Instance.CurrentRoom.PickableObjects;
        CanvasGroup[] fields = PanelPickableList.GetComponentsInChildren<CanvasGroup>(true);
        List<Text> allTexts = new List<Text>();

        int i = 1;  // because first canvas group is panel root object
        foreach(PickableObject obj in pickablesOnLevel)
        {
            fields[i].gameObject.SetActive(true);
            Button button = fields[i].GetComponentInChildren<Button>();
            Text text = fields[i].GetComponentInChildren<Text>();
            text.text = obj.Name;

            // Assigning list element to pickable object here
            obj.AssociatedListElement = fields[i];
            button.enabled = true;

            if(GameManager.Instance.CurrentRoom.PickablePickedObjectIDs.Contains(obj.ID))
            {
                ChangeTextPickedStatus(text, button, true);
            }
            else
            {
                ChangeTextPickedStatus(text, button, false);
            }

            _allPickablesDict.Add(obj, text);
            allTexts.Add(text);
            ++i;
        }


        // shutting down remaining fields
        for(int j = i; j < fields.Length; ++j)
        {
            fields[j].gameObject.SetActive(false);
            fields[j].GetComponentInChildren<Text>().text = "";
            fields[j].GetComponentInChildren<Button>().enabled = false;
        }

        LangManager.UpdateIndexToText(allTexts);
        LanguageManager.Language _lang = (LanguageManager.Language)PlayerPrefs.GetInt("PP_LANGUAGE", 0);
        LangManager.ChangeLanguage(_lang);
    }

    #endregion
}
