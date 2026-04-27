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
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseover == true)
            {
                Debug.Log("started clash");
                GameManager.gm.Clash();
            }
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //
    //    if (mouseover == true)
    //    {
    //        Debug.Log("started clash");
    //        GameManager.gm.Clash();
    //    }
    //}

    public void OnMouseOver()
    {
        

        mouseover = true;
    }

    public void OnMouseExit()
    {
        mouseover = false;
    }
}
