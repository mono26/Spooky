using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantPointDetect
{
    //Reference variable to the player class, so you can acces the variables, like position, rigidbody, etc.
    public Spooky spooky;
    //SphereCollider for detecting the collision with the plantPoints
    public SphereCollider sphereTrigger;

    public float detectRange;

    //Value of the current plantPoint. It changes through time by collision detection
    //Also the value can be null if the current plantPoint is out of the collider boundaries
    public PlantPoint currentPlantPoint;

    public PlantPoint CurrentPlantPoint
    {
        get { return currentPlantPoint; }
        set { currentPlantPoint = value; }
    }

    public PlantPointDetect(Spooky _spooky, SphereCollider _sphereTrigger, float _detectRange)
    {
        spooky = _spooky;
        sphereTrigger = _sphereTrigger;
        detectRange = _detectRange;
    }

    private void Awake()
    {
        //This component is founf on the ParentObject because this object is child of the Spooky main object
        //so we can have multiple trigger colliders for multiple collision detection purposes so each collider
        //manages it own collisions. This one only manges plantPoint collisions.
        //sphereTrigger = GetComponent<SphereCollider>();
    }
    public void Start()
    {
        //When the game starts we give the collider its radius value
        sphereTrigger.radius = detectRange;
    }

    public void Update()
    {
        if (!currentPlantPoint && PlantStore.Instance.currentPlantPoint)
        {
            PlantStore.Instance.DeselectBuildPoint();
            PlantStore.Instance.DeselectPlantPoint();
        }
        if (currentPlantPoint)
        {
            if (Vector3.SqrMagnitude(spooky.transform.position - currentPlantPoint.transform.position) >
                detectRange * detectRange)
            {
                ClearPlantPoint();
            }
        }
        else return;
    }

    //This method checks if the current plantPoint has a plant on it or not
    //depending on the value it tell the UIManager wich canvas deactivate
    private void ClearPlantPoint()
    {
        if (CurrentPlantPoint)
        {
            if (CurrentPlantPoint.currentPlant)
                PlantStore.Instance.DeselectPlantPoint();
            else if (!CurrentPlantPoint.currentPlant)
                PlantStore.Instance.DeselectBuildPoint();
            CurrentPlantPoint = null;
        }
        else return;
    }

    //This are the built-in methos for detecting trigger collision.
    //Here is called each time an object enters in collision with the sphereCollider
    public void OnTriggerEnter(Collider collider)
    {
        //If the object that entered collision is tagged as PlantPoint. Here for telling the
        //script to not check any collision that is not a plantPoint
        if (collider.CompareTag("Plant Point"))
        {
            if (CurrentPlantPoint)
            {
                PlantStore.Instance.DeselectBuildPoint();
                PlantStore.Instance.DeselectPlantPoint();
            }
            CurrentPlantPoint = collider.gameObject.GetComponent<PlantPoint>();
            if (CurrentPlantPoint.currentPlant)
                PlantStore.Instance.SelectPlantPoint(CurrentPlantPoint);
            else if (!currentPlantPoint.currentPlant)
                PlantStore.Instance.SelectBuildPoint(CurrentPlantPoint);
        }
        else return;
    }

    //Is called each time a colliders exits the bounds of the sphereCollider
    //Each time a plantPoint exits the bouds it checks if its equal to the current one
    public void OnTriggerExit(Collider collider)
    {
        //If the tag matches PlantPoint it continues to the next step, else it cancels execution
        if (collider.CompareTag("Plant Point"))
        {
            //If the collider that exited the bounds is equal to the current it calls the ClearPlantPoint() method
            if (currentPlantPoint && currentPlantPoint == collider.GetComponent<PlantPoint>())
            {
                ClearPlantPoint();
            }
        }
        else return;
    }
}
