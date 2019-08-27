using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyPlantPointSelection : CharacterComponent
{
    public override void EveryFrame()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100,1 << 17))
            {
                if(hit.transform.tag == "Plantpoint")
                {
                    
                }
            }
        }
    }
}
