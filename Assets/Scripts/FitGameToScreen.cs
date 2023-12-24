using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitGameToScreen : MonoBehaviour
{
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, 0);
        Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = _mainCamera.ViewportToWorldPoint(new Vector3(_mainCamera.rect.width, _mainCamera.rect.height)) ;
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);
        }
        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / desiredRatio);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
