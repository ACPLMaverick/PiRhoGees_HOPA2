using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hole : MonoBehaviour {

    public Image MyLoaderImage;
    public bool IsFixed = false;

    private Sprite _baseSprite;
    private BoxCollider2D _myBoxCollider;

	// Use this for initialization
	void Start () {
        InputManager.Instance.OnInputHold.AddListener(OnHold);
        InputManager.Instance.OnInputClickUp.AddListener(OnPostHold);

        _baseSprite = GetComponent<SpriteRenderer>().sprite;

        _myBoxCollider = GetComponent<BoxCollider2D>();
        _myBoxCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if(ConservationProgressManager.Instance.ClearingFinished)
        {
            _myBoxCollider.enabled = true;
        }
	}

    public void OnHold(Vector2 currentScreenPos, Collider2D hitCollider2D)
    {
        if (hitCollider2D != null && hitCollider2D.gameObject == gameObject && !IsFixed)
        {
            MyLoaderImage.fillAmount += Time.deltaTime;

            if(MyLoaderImage.fillAmount >= 1.0f)
            {
                IsFixed = true;
                ConservationProgressManager.Instance.HolesFixedCount += 1;
                MyLoaderImage.fillAmount = 0.0f;
                GetComponent<SpriteRenderer>().sprite = null;
                _myBoxCollider.enabled = false;
            }
        }
    }

    public void OnPostHold(Vector2 currentScreenPos, Collider2D hitCollider2D)
    {
        if (hitCollider2D != null && hitCollider2D.gameObject == gameObject && !IsFixed)
        {
            print("END");
            MyLoaderImage.fillAmount = 0.0f;
        }
    }

    public void Reset()
    {
        IsFixed = false;
        GetComponent<SpriteRenderer>().sprite = _baseSprite;
        _myBoxCollider.enabled = false;
    }
}
