using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MinigameProgressManager<T> : MonoBehaviour
{
    public T[] Elements;
    public int ElementsCompleted;
    public bool HasWon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!HasWon)
        {
            CheckProgress();
        }

        if (Input.GetKey(KeyCode.R))
        {
            ResetGame();
        }
    }

    public virtual void CheckProgress()
    {
        if (ElementsCompleted >= Elements.Length)
        {
            WinGame();
        }
    }

    public virtual void WinGame()
    {
        HasWon = true;
        Debug.Log("Hooray! You won!");
    }

    public virtual void ResetGame()
    {
        ElementsCompleted = 0;
        HasWon = false;
    }
}
