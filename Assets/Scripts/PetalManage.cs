using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PetalManage : MonoBehaviour {

	private RuntimePlatform platform = Application.platform;

    private bool dragging = false;
    private GameObject draggedPetal;
    private Vector2 touchOffset;

    public Text lovesMe;
    public Text lovesMeNot;
    public Text end;
    public Canvas renderCanvas;

    private int petalsPicked = 0;
    private bool done = false;
 
     void Update(){

        checkEnd();

        // rotate
        transform.Rotate(0, 0, 5 * Time.deltaTime);

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
 
    // get current touch position
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
            draggedPetal.transform.position = touchPos + touchOffset;

            // change color
            Color color = draggedPetal.transform.gameObject.GetComponent<SpriteRenderer>().color;
            color.a -= 0.02f;
            draggedPetal.transform.gameObject.GetComponent<SpriteRenderer>().color = color;
        }
        else{
            Collider2D hit = Physics2D.OverlapPoint(touchPos);
 
            // check if this gameObject
            if(hit){
                if(hit.transform != null){
                dragging = true;
                draggedPetal = hit.transform.gameObject;
                touchOffset = (Vector2)hit.transform.position - touchPos;
                }
            }
        }
     }

     void remove(){
        dragging = false;
        petalsPicked++;

        Text textToInstantiate;
        Vector3 pos;

        //instantiate text
        // which text
        if(petalsPicked % 2 == 0){
            textToInstantiate = lovesMeNot;  
        }
        else{
            textToInstantiate = lovesMe;   
        }

        // location of text
        if(Random.value > 0.5f){
             pos = new Vector3(Random.Range(-120.0f,120.0f),Random.Range(-550.0f,-350.0f),0);
        }
        else{
            pos = new Vector3(Random.Range(-120.0f,120.0f),Random.Range(550.0f,350.0f),0);
        }

        Text newText = (Text) Instantiate(textToInstantiate, pos, Quaternion.identity);
        newText.transform.SetParent(renderCanvas.transform, false);
        StartCoroutine(fadeOutText(newText));

        // remove petal
        Destroy(draggedPetal.transform.gameObject);
     }

     IEnumerator fadeOutText(Text text){
        float f = 1f;
       while(f >= 0f) {
            Color c = text.color;
            c.a = f;
            text.color = c;
            f -= 0.05f;
            yield return new WaitForSeconds(.2f);
        }
        text.enabled = false;
     }

     IEnumerator fadeInText(Text text){
        float f = 0f;
       while(f <= 1f) {
            Color c = text.color;
            c.a = f;
            text.color = c;
            f += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
     }

     IEnumerator fadeOutSprite(GameObject sprite){
        float f = 1f;
       while(f >= 0f) {
            Color color = sprite.transform.gameObject.GetComponent<SpriteRenderer>().color;
            color.a = f;
            sprite.transform.gameObject.GetComponent<SpriteRenderer>().color = color;
            f -= 0.05f;
            yield return new WaitForSeconds(.2f);
        }
        sprite.GetComponent<SpriteRenderer>().enabled = false;
     }

     IEnumerator fadeOutFlower(){
        
        foreach(Transform child in transform){
            StartCoroutine(fadeOutSprite(child.gameObject));
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(fadeOutSprite(transform.gameObject));

        done = true;
    }

     void checkEnd(){
        bool isEnd = true;

        if(transform.childCount <= 3){
            foreach (Transform child in transform){
                if(child.tag != "leaf"){
                    isEnd = false;
                }
            }

            if(isEnd == true){
                if(petalsPicked % 2 == 0){
                    end.text = "He loves me not.";
                }
                else{
                    end.text = "He loves me.";
                }
                if(done == false){
                    StartCoroutine(fadeInText(end));
                    StartCoroutine(fadeOutFlower());
                }           
            }
            
        }

        //reload
        if(transform.gameObject.GetComponent<SpriteRenderer>().enabled == false){ 
            StartCoroutine(reload());
        }
     }

     IEnumerator reload(){
        yield return new WaitForSeconds(4f);
        Application.LoadLevel(Application.loadedLevel);
     }
}