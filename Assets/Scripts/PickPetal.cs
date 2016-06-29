using UnityEngine;
using System.Collections;

public class PickPetal : MonoBehaviour {

	private RuntimePlatform platform = Application.platform;

    private bool dragging = false;
    //private gameObject draggedPetal;
    private Vector2 touchOffset;
 
     void Update(){

         if(hasInput){
            followTouch();
         }
         else{
            if(dragging){
                remove();
            }
         }
     }

     // check for input
     private bool hasInput{
        get{
            if(platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer){
                if(Input.touchCount > 0) {
                    return true;
                }
                return false;
            }
            else if(platform == RuntimePlatform.WindowsEditor){
                return Input.GetMouseButton(0);
            }
            return false;
        }
    }
 
    Vector2 currentTouchPos{
        get{
            Vector2 inputPos;
            if(platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer){
                inputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                return inputPos;
            }
            else if(platform == RuntimePlatform.WindowsEditor){
                inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return inputPos;
            }
            inputPos = new Vector2(1f,1f);
            return inputPos;
        }
    }

     void followTouch(){
        Vector2 touchPos = currentTouchPos;

        if(dragging){
            transform.position = touchPos + touchOffset;

            // change color
            Color color = transform.gameObject.GetComponent<SpriteRenderer>().color;
            color.a -= 0.05f;
            transform.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        else{
            Collider2D hit = Physics2D.OverlapPoint(touchPos);
 
            // check if this gameObject
            if(hit){
                if(hit.transform.gameObject == transform.gameObject){
                dragging = true;
                touchOffset = (Vector2)hit.transform.position - touchPos;
                }
            }
        }
     }

     void remove(){
        
        dragging = false;
        Destroy(transform.gameObject);
     }
}