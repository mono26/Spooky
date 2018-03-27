using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantPointDetect : IDetect
{
    //Reference variable to the player class, so you can acces the variables, like position, rigidbody, etc.
    private Spooky spooky;
    private Settings settings;

    //Value of the current plantPoint. It changes through time by collision detection
    //Also the value can be null if the current plantPoint is out of the collider boundaries
    private Plantpoint currentPlantPoint;
    [SerializeField]
    private List<Plantpoint> nearPlantPoints;

    public PlantPointDetect(Spooky _spooky, Settings _settings)
    {
        spooky = _spooky;
        settings = _settings;

        nearPlantPoints = new List<Plantpoint>();
    }

    public void Start()
    {
        //When the game starts we give the collider its radius value
        settings.SphereTrigger.radius = settings.DetectRange;
        spooky.StartCoroutine(DisplayNearestPlantPoint());
    }

    public void Detect()
    {
        if (!currentPlantPoint && PlantStore.Instance.CurrentPlantPoint)
        {
            PlantStore.Instance.DeselectBuildPoint();
            PlantStore.Instance.DeselectPlantPoint();
            return;
        }

        if (currentPlantPoint)
        {
            if (Vector3.SqrMagnitude(
                spooky.transform.position - currentPlantPoint.transform.position
                ) > settings.DetectRange * settings.DetectRange)
            {
                ClearCurrentPlantPoint();
                return;
            }
        }

        return;
    }

    private IEnumerator DisplayNearestPlantPoint()
    {
        if (nearPlantPoints.Count > 0)
        {
            var distance1 = Vector3.SqrMagnitude(spooky.transform.position - nearPlantPoints[0].transform.position);
            var tempPlantPoint = nearPlantPoints[0];
            for (int plantPoint = 0; plantPoint < nearPlantPoints.Count -1; plantPoint++)
            {
                var distance2 = Vector3.SqrMagnitude(spooky.transform.position - nearPlantPoints[plantPoint].transform.position);
                if (distance2 < distance1)
                {
                    distance1 = distance2;
                    tempPlantPoint = nearPlantPoints[plantPoint];
                }
            }

            yield return new WaitForSeconds(settings.UpdateRate);

            SelectPlantPoint(tempPlantPoint);
        }

        yield return new WaitForSeconds(settings.UpdateRate);

        spooky.StartCoroutine(DisplayNearestPlantPoint());
    }

    //This method checks if the current plantPoint has a plant on it or not
    //depending on the value it tell the UIManager wich canvas to deactivate
    private void ClearCurrentPlantPoint()
    {
        if (currentPlantPoint)
        {
            if (currentPlantPoint.currentPlant)
                PlantStore.Instance.DeselectPlantPoint();
            else if (!currentPlantPoint.currentPlant)
                PlantStore.Instance.DeselectBuildPoint();
            nearPlantPoints = null;
            return;
        }
        else return;
    }

    private void SelectPlantPoint(Plantpoint _plantPoint)
    {
        currentPlantPoint = _plantPoint;
        if (currentPlantPoint.currentPlant)
        {
            PlantStore.Instance.SelectPlantPoint(currentPlantPoint);
            return;
        }
        else if (!currentPlantPoint.currentPlant)
        {
            PlantStore.Instance.SelectBuildPoint(currentPlantPoint);
            return;
        }
    }

    private void AddPlantPoint(Plantpoint _plantPoint)
    {
        if (nearPlantPoints.Count == 0 || !nearPlantPoints.Contains(_plantPoint))
        {
            var time = Time.deltaTime;
            Debug.Log(time + " adding plantpoint. " + _plantPoint.ToString());
            nearPlantPoints.Add(_plantPoint);
            return;
        }
        else return;
    }

    private void RemovePlantPoint(Plantpoint _plantPoint)
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
    public void OnTriggerEnter(Collider _plantPoint)
    {
        //If the object that entered collision is tagged as PlantPoint. Here for telling the
        //script to not check any collision that is not a plantPoint
        if (_plantPoint.CompareTag("Plantpoint"))
        {
            var plantPoint = _plantPoint.GetComponent<Plantpoint>();
            if (!currentPlantPoint)
            {
                SelectPlantPoint(plantPoint);
            }

            AddPlantPoint(plantPoint);
            return;
        }
        else return;
    }

    //Is called each time a colliders exits the bounds of the sphereCollider
    //Each time a plantPoint exits the bouds it checks if its equal to the current one
    public void OnTriggerExit(Collider _plantPoint)
    {
        //If the tag matches PlantPoint it continues to the next step, else it cancels execution
        if (_plantPoint.CompareTag("Plantpoint"))
        {
            //If the collider that exited the bounds is equal to the current it calls the ClearPlantPoint() method
            var plantPoint = _plantPoint.GetComponent<Plantpoint>();
            if (currentPlantPoint && currentPlantPoint == plantPoint)
            {
                ClearCurrentPlantPoint();
            }

            RemovePlantPoint(plantPoint);
            return;
        }
        else return;
    }

    [System.Serializable]
    public class Settings
    {
        //SphereCollider for detecting the collision with the plantPoints
        public SphereCollider SphereTrigger;
        [Range(1.5f, 2f)]
        public float DetectRange;
        public float UpdateRate;
    }
}
