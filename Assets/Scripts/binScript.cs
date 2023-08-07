using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class binScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public uiManagerScript manager;

    void Start()
    {
        manager = GameObject.Find("inGameUI(Clone)").GetComponent<uiManagerScript>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.showBinToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.hideBinToolTip();
    }

    public void binAbility()
    {
        manager.binAbility();
    }
}