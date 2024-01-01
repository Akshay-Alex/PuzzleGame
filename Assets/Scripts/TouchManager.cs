using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    int layerMask = 1 << 6;

    // Update is called once per frame
    void Update()
    {
        ManageTouch();
    }
    void ManageTouch()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPos = touch.position;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction,Mathf.Infinity, layerMask);
                if (hit.collider != null && hit.transform == transform)
                {
                   var touchedTile = hit.collider.gameObject.GetComponent<Tile>();
                    if(touchedTile)
                    {
                        if(touch.phase == TouchPhase.Began )
                        {
                           // touchedTile.OnTouchStartedOrEnded();
                        }
                        else if(touch.phase == TouchPhase.Moved)
                        {
                            //touchedTile.OnTouchMoved();
                        }
                        else if(touch.phase == TouchPhase.Ended)
                        {
                            //touchedTile.OnTouchStartedOrEnded();
                        }
                    }
                }
            }
        }
    }
}
