using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonEquipmentPanelToggle : MonoBehaviour
{
    #region public

    public Sprite SpriteEquipment;
    public Sprite SpriteList;

    #endregion

    #region private

    #endregion

    #region functions

    protected virtual void Awake()
    {
    }

    // Use this for initialization
    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //public void SwitchMode(EquipmentManager.EquipmentMode mode)
    //{
    //    switch (mode)
    //    {
    //        case EquipmentManager.EquipmentMode.PICKABLES:

    //            _img.sprite = SpriteEquipment;

    //            break;
    //        case EquipmentManager.EquipmentMode.USABLES:

    //            _img.sprite = SpriteList;

    //            break;
    //    }
    //}

    #endregion
}
