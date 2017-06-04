using UnityEngine;
using System.Collections;

public class OriginalPhoto : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected float _baseScale = 0.25f;

    [SerializeField]
    protected float _enlargedScale = 1.0f;

    [SerializeField]
    protected float _enlargingSpeed = 1.0f;

    bool _bIsClicked = false;

    #endregion

    #region Protected

    #endregion

    #region MonoBehaviours

    // Use this for initialization
    void Start ()
    {
        transform.localScale = new Vector3(_baseScale, _baseScale, _baseScale);
        InputManager.Instance.OnInputClickUp.AddListener(OnClickUp);
        InputManager.Instance.OnInputClickDown.AddListener(OnClickDown);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 scaleLerpTo = new Vector3(_baseScale, _baseScale, _baseScale);
	    if(_bIsClicked)
        {
            scaleLerpTo = new Vector3(_enlargedScale, _enlargedScale, _enlargedScale);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, scaleLerpTo, Time.deltaTime * _enlargingSpeed);
	}

    #endregion

    #region Functions 

    protected void OnClickUp(Vector2 screenPos, Collider2D hitCollider2D)
    {
        if (gameObject != null &&
            hitCollider2D != null &&
            hitCollider2D.gameObject == gameObject)
        {
            _bIsClicked = false;
        }
    }

    protected void OnClickDown(Vector2 screenPos, Collider2D hitCollider2D)
    {
        if (gameObject != null &&
            hitCollider2D != null &&
            hitCollider2D.gameObject == gameObject)
        {
            _bIsClicked = true;
        }
    }

    #endregion

}
