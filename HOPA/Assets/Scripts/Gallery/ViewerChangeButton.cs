using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewerChangeButton : MonoBehaviour
{
    public ViewerScreen Screen { get; set; }
    public Viewer ViewerRef { get; set; }

	// Use this for initialization
	void Start ()
    {
        GetComponentInChildren<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(OnClick));
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    protected void OnClick()
    {
        ViewerRef.ChangeScreen(Screen);
    }
}
