using UnityEngine;
using UnityEngine.EventSystems;

public class CardBody : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Card parent;
    bool mouseover = false;
    void Awake()
    {
        parent = GetComponentInParent<Card>();
        
        if (parent != null)
        {
            Debug.Log("Parent card found: " + parent.card_name);
        }
        else
        {
            Debug.LogError("No parent card found on CardBody: " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parent == null)
            return;

        if (mouseover)
        {
            parent.OnCardClicked(); // Call the method on the parent card
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parent == null)
        {
            Debug.LogError("Pointer enter but parent is null on CardBody: " + gameObject.name);
            return;
        }

        Debug.Log("Pointer enter CardBody for card: " + parent.card_name);
        parent.HighlightCard(); // Call the highlight method on the parent
        mouseover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parent == null)
            return;

        parent.UnhighlightCard(); // Call the unhighlight method on the parent
        mouseover = false;
    }
}
