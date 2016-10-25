﻿using UnityEngine;
using System.Collections;

public class PuzzleProgressManager : MinigameProgressManager<PuzzleElement> {

    #region Public

    public static PuzzleProgressManager Instance;

    #endregion

    #region Functions

    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    public override void ResetGame()
    {
        base.ResetGame();

        foreach(PuzzleElement puzzle in Elements)
        {
            puzzle.ResetElement();
        }
    }

    #endregion
}
