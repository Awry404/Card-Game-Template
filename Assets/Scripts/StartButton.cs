using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour
{
    public bool mouseover = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    
        if (mouseover)
        {
            Debug.Log("started clash");
            GameManager.gm.Clash();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        

        mouseover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseover = false;
    }
}
