using UnityEngine;
using System.Collections;

public class Dust : MonoBehaviour {

    public int ClearingRadius;
    public bool IsClear;

    private BoxCollider2D _myCollider;
    private Texture2D _myTexture;
    private SpriteRenderer _mySpriteRenderer;
    private Texture2D _copy;
    private Vector3 _wp1;

	// Use this for initialization
	void Start () {
        InputManager.Instance.OnInputMoveExclusive.AddListener(OnDrag);

        _myCollider = GetComponent<BoxCollider2D>();
        _mySpriteRenderer = this.GetComponent<SpriteRenderer>();

        _myTexture = (GetComponent<SpriteRenderer>().sprite.texture as Texture2D);

        CopyTexture2D();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void OnDrag(Vector2 currentScreenPos, Vector2 direction, Collider2D hitCollider2D)
    {
        if (hitCollider2D != null && hitCollider2D.gameObject == gameObject && !IsClear)
        {
            _wp1 = Camera.main.ScreenToWorldPoint(currentScreenPos);

            //print(_wp1);

            UpdateTexture();
        }
    }

    public void UpdateTexture()
    {
        //LOGIC
        if(_wp1.x <= this.transform.position.x + (_myCollider.size.x * this.transform.localScale.x) / 2 &&
            _wp1.x >= this.transform.position.x - (_myCollider.size.x * this.transform.localScale.x) / 2 &&
            _wp1.y <= this.transform.position.y + (_myCollider.size.y * this.transform.localScale.y) / 2 &&
            _wp1.y >= this.transform.position.y - (_myCollider.size.y * this.transform.localScale.y) / 2)
        {
            _wp1 -= this.transform.position;

            ClearWithBrush(Mathf.RoundToInt((_wp1.x + 1) * _copy.width * 0.5f),
                Mathf.RoundToInt((_wp1.y + 1) * _copy.height * 0.5f));
        }

        //This finalizes it. If you want to edit it still, do it before you finish with Apply(). Do NOT expect to edit the image after you have applied.
        _copy.Apply();
    }

    public void ClearWithBrush(int baseX, int baseY)
    {
        for(int y = baseY - ClearingRadius; y <= baseY + ClearingRadius; ++y)
        {
            for(int x = baseX - ClearingRadius; x <= baseX + ClearingRadius; ++x)
            {
                if (y <= _copy.height &&
                    y >= 0 &&
                    x <= _copy.width &&
                    x >= 0)
                {
                    if (_copy.GetPixel(x, y).a != 0)
                    {
                        _copy.SetPixel(x, y, new Color(0, 0, 0, 0));
                        ConservationProgressManager.Instance.ElementsCompleted += 1;
                    }
                }
            }
        }
    }

    public void CopyTexture2D()
    {
        //Create a new Texture2D, which will be the copy
        _copy = new Texture2D(_myTexture.width, _myTexture.height);
        
        //Choose your filtermode and wrapmode
        _copy.filterMode = FilterMode.Bilinear;
        _copy.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < _myTexture.width; ++i)
        {
            for (int j = 0; j < _myTexture.height; ++j)
            {
                _copy.SetPixel(i, j, _myTexture.GetPixel(i, j));
            }
        }
        _copy.Apply();

        //Get the name of the old sprite
        string tempName = _mySpriteRenderer.sprite.name;
        //Get the inital value of PPU because it is changing because I don't know why
        float ppu = _mySpriteRenderer.sprite.pixelsPerUnit;
        //Create a new sprite
        _mySpriteRenderer.sprite = Sprite.Create(_copy, new Rect(0, 0, _myTexture.width, _myTexture.height), new Vector2(0.5f, 0.5f), ppu);
        //Name the sprite, the old name
        _mySpriteRenderer.sprite.name = tempName;

    }

    public void Reset()
    {
        IsClear = false;

        CopyTexture2D();
    }

    public int GetPixelsCount()
    {
        return _copy.width * _copy.height;
    }
}
