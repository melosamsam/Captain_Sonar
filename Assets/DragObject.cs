using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour
{
    #region Attributes

    private Canvas canvas;

    #endregion

    #region Unity methods

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    #endregion

    #region Methods

    public void DragUI(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
            );
        transform.position = canvas.transform.TransformPoint(position);
    }

    #endregion
}
