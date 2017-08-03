using UnityEngine;
using System.Collections;

public class Dust : MonoBehaviour {

    public int ClearingRadius;
    public float PixelClearedThreshold;
    public bool IsClear;

    private BoxCollider2D _myCollider;
    private Texture2D _myTexture;
    private SpriteRenderer _mySpriteRenderer;
    private Texture2D _copy;
    private Vector3 _wp1;

    private Coroutine _disappearCoroutine;
    private float _coroutineTime = 1.0f;

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

            UpdateTexture();
        }
    }

    public void UpdateTexture()
    {
        // transform wp1 to local coordinates of game object
        _wp1 = transform.worldToLocalMatrix * new Vector4(_wp1.x, _wp1.y, _wp1.z, 1.0f);
        // transform local coordinates to texture coordinates, taking sprite size into account
        _wp1.x = Mathf.Clamp01(((_wp1.x / _mySpriteRenderer.sprite.bounds.extents.x) + 1.0f) * 0.5f) * _copy.width;
        _wp1.y = Mathf.Clamp01(((_wp1.y / _mySpriteRenderer.sprite.bounds.extents.y) + 1.0f) * 0.5f) * _copy.height;

        ClearWithBrush(Mathf.RoundToInt(_wp1.x), Mathf.RoundToInt(_wp1.y));

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
                    if (_copy.GetPixel(x, y).a != 0.0f)
                    {
                        Color pixel = _copy.GetPixel(x, y);
                        int distance = CountDistance(baseX, baseY, x, y);
                        if (distance <= ClearingRadius)
                        {
                            float alpha = 2.0f / Mathf.Max(distance, 0.0001f);
                            alpha = pixel.a - alpha;
                            alpha = Mathf.Clamp01(alpha);

                            _copy.SetPixel(x, y, new Color(pixel.r, pixel.g, pixel.b, alpha));

                            if (alpha <= PixelClearedThreshold)
                            {
                                ConservationProgressManager.Instance.ElementsCompleted += 1;
                            }
                        }
                    }
                }
            }
        }
    }

    public int CountDistance(int baseX, int baseY, int targetX, int targetY)
    {
        return (int)(Mathf.Sqrt(SimpleSquarePow(targetY - baseY) + SimpleSquarePow(targetX - baseX)));
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
        _mySpriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public int GetPixelsCount()
    {
        return _copy.width * _copy.height;
    }

    public void DustDisappear()
    {
        _disappearCoroutine = StartCoroutine(DustDisappearOnComplete());
    }

    private IEnumerator DustDisappearOnComplete()
    {
        float time = 0.0f;
        float alpha = 1.0f;

        while(time < _coroutineTime)
        {
            alpha = Mathf.Lerp(1, 0, (time / _coroutineTime));
            _mySpriteRenderer.color = new Color(1, 1, 1, alpha);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private int SimpleSquarePow(int i)
    {
        return i * i;
    }
}
