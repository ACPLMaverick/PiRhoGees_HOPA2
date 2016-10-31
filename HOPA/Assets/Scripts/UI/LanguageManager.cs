using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    #region const

    const string PP_LANGUAGE = "PP_LANGUAGE";
    const string FILE_ROOT_NAME = "stringtable"; 
    const string FILE_STRING_NAME = "string";
    const string FILE_STRING_PARAM_INDEX = "index";
    const string FILE_VALUE_NAME = "value";
    const string FILE_VALUE_PARAM_LANG = "lang";
    const string FILE_LANG_PL = "pl";
    const string FILE_LANG_EN = "en";
    const string FILE_INDEX_PREFIX = "$";

    #endregion

    #region enum

    public enum Language
    {
        Polish,
        English,
        _count
    }

    #endregion

    #region Properties

    public Language CurrentLanguage
    {
        get
        {
            return _lang;
        }
    }

    #endregion

    #region protected

    [SerializeField]
    protected TextAsset _stringtable;

    [SerializeField]
    protected Button _langChangeButton;

    protected XmlDocument _xmlDoc;
    protected Dictionary<Text, string> _indexToText;
    protected Language _lang;

    #endregion

    #region MonoBehaviour

    // Use this for initialization
    void Start ()
    {
        _lang = (Language)PlayerPrefs.GetInt(PP_LANGUAGE, 0);

        _xmlDoc = new XmlDocument();
        _xmlDoc.LoadXml(_stringtable.text);

        _langChangeButton.onClick.AddListener(new UnityEngine.Events.UnityAction(ChangeLanguageToNext));

        PrepareIndexToText();
        ChangeLanguage(_lang);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnDestroy()
    {
        PlayerPrefs.SetInt(PP_LANGUAGE, (int)_lang);
    }

    #endregion

    #region Functions

    public void ChangeLanguageToNext()
    {
        ChangeLanguage((Language)(((int)_lang + 1) % (int)Language._count));
    }

    public void ChangeLanguage(Language lang)
    {
        string paramLang;
        switch(lang)
        {
            case Language.Polish:
                paramLang = FILE_LANG_PL;
                break;
            case Language.English:
                paramLang = FILE_LANG_EN;
                break;
            default:
                paramLang = FILE_LANG_PL;
                break;
        }

        XmlNodeList strings = _xmlDoc.GetElementsByTagName(FILE_STRING_NAME);
        foreach(KeyValuePair<Text, string> pair in _indexToText)
        {
            if (pair.Value[0] == '$')
            {
                for (int j = 0; j < strings.Count; ++j)
                {
                    if (strings[j].Attributes.Count > 0 &&
                        strings[j].Attributes[0].Value.Equals(pair.Value))
                    {
                        XmlNodeList values = strings[j].ChildNodes;
                        for (int k = 0; k < values.Count; ++k)
                        {
                            if (values[k].Attributes.Count > 0 &&
                                values[k].Attributes[0].Value.Equals(paramLang))
                            {
                                pair.Key.text = values[k].InnerText.Trim();
                                goto TextFinished;
                            }
                        }
                    }
                }
            }
        TextFinished:
            continue;
        }

        _lang = lang;
    }

    protected void PrepareIndexToText()
    {
        _indexToText = new Dictionary<Text, string>();
        Text[] allTexts = GetComponentsInChildren<Text>(true);

        for(int i = 0; i < allTexts.Length; ++i)
        {
            _indexToText.Add(allTexts[i], allTexts[i].text);
        }
    }

    #endregion
}
