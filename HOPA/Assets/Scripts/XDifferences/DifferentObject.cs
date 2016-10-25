using UnityEngine;
using System.Collections;

public class DifferentObject : MonoBehaviour {

    public bool IsFound;

	// Use this for initialization
	void Start () {
        InputManager.Instance.OnInputClickDown.AddListener(DetectClick);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DetectClick(Vector2 screenPos, Collider2D hitCollider2D)
    {
        IsFound = true;
        DifferencesProgressManager.Instance.ElementsCompleted += 1;
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Reset()
    {
        IsFound = false;

        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
}
