using UnityEngine;
using System.Collections;

public class PuzzleProgressManager : MonoBehaviour {

    public static PuzzleProgressManager Instance;

    public PuzzleElement[] Elements;
    public int ElementsOnSlots;
    public bool HasWon;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	    if(!HasWon)
        {
            CheckProgress();
        }

        if(Input.GetKey(KeyCode.R))
        {
            ResetGame();
        }
	}

    void CheckProgress()
    {
        if(ElementsOnSlots >= Elements.Length)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        HasWon = true;
        Debug.Log("Hooray! You won!");
    }

    void ResetGame()
    {
        ElementsOnSlots = 0;
        HasWon = false;

        foreach(PuzzleElement puzzle in Elements)
        {
            puzzle.ResetElement();
        }
    }
}
