using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class DragNDrop : MonoBehaviour
{
    bool canMove;
    bool dragging;
    bool mouseOver;
    public GameObject spell;

    
    // Start is called before the first frame update
    void Start()
    {
        
        canMove = false;
        dragging = false;
        mouseOver = false;

    }

    // Update is called once per frame
    void Update() { 
    

        if (Input.GetMouseButtonDown(0) && mouseOver == true)
        {
            canMove = false;
            dragging = false;


            // Debug.Log("Clicked");
        }

    }
    
    
     void OnMouseExit() { 
    

        Debug.Log("Mouse is off");
    }
    public void OnMouseOver()
    {

        mouseOver = true;
        Debug.Log("mouse is over");

    }

}
