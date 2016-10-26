using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifferentObject : MonoBehaviour {

    public bool IsFound;

    private Coroutine _redMarkAnimation;
    private Image _redMark;

	// Use this for initialization
	void Start () {
        InputManager.Instance.OnInputClickDown.AddListener(DetectClick);

        _redMark = GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DetectClick(Vector2 screenPos, Collider2D hitCollider2D)
    {
        if(hitCollider2D != null && hitCollider2D.gameObject == this.gameObject && !IsFound)
        {
            IsFound = true;
            DifferencesProgressManager.Instance.ElementsCompleted += 1;

            _redMarkAnimation = StartCoroutine(RedMarkAnimation());

            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void Reset()
    {
        IsFound = false;

        this.GetComponent<BoxCollider2D>().enabled = true;
        _redMark.fillAmount = 0;
    }

    public IEnumerator RedMarkAnimation()
    {
        float animationLength = 0.2f;
        float currentTime = 0.0f;

        while(currentTime <= animationLength)
        {

            _redMark.fillAmount = Mathf.Lerp(0.0f, 1.0f, currentTime / animationLength);

            currentTime += Time.fixedDeltaTime;
            yield return null;
        }
    }
}
