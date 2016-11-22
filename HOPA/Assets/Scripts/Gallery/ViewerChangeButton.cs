using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewerChangeButton : MonoBehaviour
{
    [SerializeField]
    protected Color _colorNonCurrent = new Color(0.25f, 0.25f, 0.25f);
    [SerializeField]
    protected Color _colorCurrent = Color.grey;

    public ViewerScreen Screen { get; set; }
    public Viewer ViewerRef { get; set; }
    public List<ViewerChangeButton> AllButtons { get; set; }

    public Color ColorCurrent { get { return _colorCurrent; } set { _colorCurrent = value; } } 

    void Awake()
    {
        MakeNonCurrent();
    }

	// Use this for initialization
	void Start ()
    {
        GetComponentInChildren<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(OnClick));
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void MakeCurrent()
    {
        GetComponentInChildren<Text>().color = _colorCurrent;
    }

    public void MakeNonCurrent()
    {
        GetComponentInChildren<Text>().color = _colorNonCurrent;
    }

    protected void OnClick()
    {
        for(int i = 0; i < AllButtons.Count; ++i)
        {
            if(AllButtons[i] != this)
            {
                AllButtons[i].MakeNonCurrent();
            }
        }
        MakeCurrent();
        ViewerRef.ChangeScreen(Screen);
    }

}
