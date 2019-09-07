using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = null;

    private void Awake() 
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] clickedObjects = Physics.RaycastAll(mouseRay, 100.0f);
            if (clickedObjects.Length > 0)
            {
                for (int i = 0; i < clickedObjects.Length; i++)
                {
                    if (clickedObjects[i].collider.CompareTag("PlantPoint"))
                    {
                        clickedObjects[i].collider.GetComponent<Plantpoint>().OnClicked();
                    }
                }
            }
        }
    }
}
