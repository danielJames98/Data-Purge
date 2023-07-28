using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class uiIconScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public uiManagerScript manager;
    public int abilityNum;
    public bool equipped;
    public bool hovered;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("inGameUI(Clone)").GetComponent<uiManagerScript>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.showToolTip(abilityNum,equipped);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.hideToolTip();
    }

    public void selectAbility()
    {
        manager.selectAbility(abilityNum,equipped);
    }
}
