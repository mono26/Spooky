using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyPlantPointDetection : CharacterComponent
{
    //SphereCollider for detecting the collision with the plantPoints
    [SerializeField]
    protected SphereCollider detectionTrigger;
    [SerializeField]
    [Range(1.5f, 2.5f)]
    protected float detectRange = 2.5f;

    //Value of the current plantPoint. It changes through time by collision detection
    //Also the value can be null if the current plantPoint is out of the collider boundaries
    private Coroutine lookForClosestPlantpoint = null;
    [SerializeField]
    private Plantpoint currentPlantPoint;
    [SerializeField]
    private List<Plantpoint> nearPlantPoints = null;

    protected override void Awake()
    {
        base.Awake();
        
        if(detectionTrigger == null)
            detectionTrigger = transform.Find("PlantPointDetector").GetComponent<SphereCollider>();

        nearPlantPoints = new List<Plantpoint>();
    }

    protected void Start()
    {
        //When the game starts we give the collider its radius value
        detectionTrigger.radius = detectRange;
    }

    public override void EveryFrame()
    {
        Detect();
    }

    public void Detect()
    {
        if (!currentPlantPoint && PlantStore.Instance.CurrentPlantPoint)
        {
            PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();
            PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
            return;
        }

        return;
    }

    private IEnumerator DisplayNearestPlantPoint()
    {
        if (nearPlantPoints.Count > 0)
        {
            var distance1 = Vector3.SqrMagnitude(character.CharacterTransform.position - nearPlantPoints[0].transform.position);
            var tempPlantPoint = nearPlantPoints[0];
            for (int plantPoint = 0; plantPoint < nearPlantPoints.Count; plantPoint++)
            {
                var distance2 = Vector3.SqrMagnitude(character.CharacterTransform.position - nearPlantPoints[plantPoint].transform.position);
                if (distance2 < distance1)
                {
                    distance1 = distance2;
                    tempPlantPoint = nearPlantPoints[plantPoint];
                }
            }

            if(currentPlantPoint != null && !currentPlantPoint.Equals(tempPlantPoint))
            {
                SelectNewPlantpoint(tempPlantPoint);
            }
        }
        yield return new WaitForSeconds(Time.deltaTime);
        lookForClosestPlantpoint = StartCoroutine(DisplayNearestPlantPoint());
    }

    //This method checks if the current plantPoint has a plant on it or not
    //depending on the value it tell the UIManager wich canvas to deactivate
    private void ClearCurrentPlantPoint()
    {
        if (currentPlantPoint == true)
        {
            if (currentPlantPoint.CurrentPlant == true)
                PlantStore.Instance.DeselectCurrentActivePlantpointWithPlant();
            else if (currentPlantPoint.CurrentPlant == false)
                PlantStore.Instance.DeselectCurrentActiveEmptyPlantpoint();

            currentPlantPoint = null;
        }

        else return;
    }

    private void SelectNewPlantpoint(Plantpoint _plantPoint)
    {
        // To prevent flickering of the UI
        if (currentPlantPoint == false)
        {
            ChangeCurrentPlantPointAndDisplay(_plantPoint);
        }
        else if (currentPlantPoint && currentPlantPoint.Equals(_plantPoint) == false)
        {
            ClearCurrentPlantPoint();
            ChangeCurrentPlantPointAndDisplay(_plantPoint);
        }
        else return;
    }

    private void ChangeCurrentPlantPointAndDisplay(Plantpoint _plantPoint)
    {
        currentPlantPoint = _plantPoint;
        if (currentPlantPoint.CurrentPlant == true)
        {
            PlantStore.Instance.ActivatePlantUI(currentPlantPoint);
            return;
        }
        else if (currentPlantPoint.CurrentPlant == false)
        {
            PlantStore.Instance.ActivateBuildUI(currentPlantPoint);
            return;
        }
    }

    private void AddPlantPointToTheList(Plantpoint _plantPoint)
    {
        if (nearPlantPoints.Count == 0 || !nearPlantPoints.Contains(_plantPoint))
        {
            nearPlantPoints.Add(_plantPoint);
            return;
        }
        else return;
    }

    private void RemovePlantPointFromTheList(Plantpoint _plantPoint)
    {
        if (nearPlantPoints.Count > 0 || nearPlantPoints.Contains(_plantPoint))
        {
            nearPlantPoints.Remove(_plantPoint);
            return;
        }
        else return;
    }

    //This are the built-in methos for detecting trigger collision.
    //Here is called each time an object enters in collision with the sphereCollider
    public void OnTriggerEnter(Collider _collider)
    {
        //If the object that entered collision is tagged as PlantPoint. Here for telling the
        //script to not check any collision that is not a plantPoint
        if (_collider.CompareTag("Plantpoint"))
        {
            var plantPoint = _collider.GetComponent<Plantpoint>();
            if (currentPlantPoint == null)
            {
                SelectNewPlantpoint(plantPoint);
                lookForClosestPlantpoint = StartCoroutine(DisplayNearestPlantPoint());
            }

            AddPlantPointToTheList(plantPoint);
            return;
        }
        else return;
    }

    //Is called each time a colliders exits the bounds of the sphereCollider
    //Each time a plantPoint exits the bouds it checks if its equal to the current one
    public void OnTriggerExit(Collider _collider)
    {
        //If the tag matches PlantPoint it continues to the next step, else it cancels execution
        if (_collider.CompareTag("Plantpoint"))
        {
            //If the collider that exited the bounds is equal to the current it calls the ClearPlantPoint() method
            var plantPoint = _collider.GetComponent<Plantpoint>();
            if (currentPlantPoint && currentPlantPoint == plantPoint)
            {
                ClearCurrentPlantPoint();
            }

            RemovePlantPointFromTheList(plantPoint);

            if(nearPlantPoints.Count == 0)
            {
                StopCoroutine(lookForClosestPlantpoint);
            }
            return;
        }
        else return;
    }
}
