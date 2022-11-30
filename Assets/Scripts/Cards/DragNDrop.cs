using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    bool canMove;
    bool dragging;
    bool mouseOver;
    public GameObject spell;

    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        
        collider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;
        mouseOver = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
        
            if (collider == Physics2D.OverlapPoint(mousePos))
            {
                canMove = true;
                Debug.Log("move");
            }
            else
            {
                canMove = false;
            }
            if (canMove)
            {
                dragging = true;
            }
            if (dragging)
            {
                this.transform.position = mousePos;
            }
        
        
        if (Input.GetMouseButtonDown(0))
        {
            canMove = false ;
            dragging = false;
            Debug.Log("Clicked");
        }
        
    }
    void OnMouseOver()
    {
        mouseOver = true;
        Debug.Log("mouse is over");
        
    }
   void OnMouseExit()
    {
        Debug.Log("Mouse is off");
    }
    
}
