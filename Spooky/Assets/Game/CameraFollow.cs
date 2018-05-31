using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Distances")]
    /// How far ahead from the Player the camera is supposed to be		
    public float HorizontalLookDistance = 3;
    /// Vertical Camera Offset	
    public Vector3 CameraOffset;
    /// Minimal distance that triggers look ahead
    public float LookAheadTrigger = 0.1f;

    [Header("Movement Speed")]
    /// How fast the camera goes back to the Player
    public float ResetSpeed = 0.5f;
    /// How fast the camera moves
    public float CameraSpeed = 0.3f;

    [Header("Dependencies")]
    [SerializeField]
    protected Camera _camera;
    [SerializeField]
    protected Transform _target;
    [SerializeField]
    protected Character _character;
    protected Bounds _levelBounds;

    protected float _xMin;
    protected float _xMax;
    protected float _yMin;
    protected float _yMax;

    protected float _offsetY;
    protected Vector3 _lastTargetPosition;
    protected Vector3 _currentVelocity;
    protected Vector3 _lookAheadPos;

    protected Vector3 _lookDirectionModifier = new Vector3(0, 0, 0);

    protected void Awake()
    {
        _camera = GetComponent<Camera>();
        _character = _target.GetComponent<Character>();

        _lastTargetPosition = _target.position;
        _offsetY = (transform.position - _target.position).y;
        transform.parent = null;

        return;
    }

    protected void LateUpdate()
    {
        //GetLevelBounds();

        if (_character != null)
            FollowPlayer();

        return;
    }

    protected virtual void FollowPlayer()
    {
        // if the player has moved since last update
        float xMoveDelta = (_target.position - _lastTargetPosition).x;
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadTrigger;

        if (updateLookAheadTarget)
        {
            _lookAheadPos = HorizontalLookDistance * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime * ResetSpeed);
        }

        Vector3 aheadTargetPos = _target.position + _lookAheadPos + Vector3.up * _offsetY + _lookDirectionModifier + CameraOffset;
        Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, CameraSpeed);

        if (_camera.orthographic == true)
        {
            float posX, posY, posZ = 0f;
            // Clamp to level boundaries
            if (_levelBounds.size != Vector3.zero)
            {
                posX = Mathf.Clamp(newCameraPosition.x, _xMin, _xMax);
                posY = Mathf.Clamp(newCameraPosition.y, _yMin, _yMax);
            }
            else
            {
                posX = newCameraPosition.x;
                posY = newCameraPosition.y;
            }
            posZ = newCameraPosition.z;
            // We move the actual transform
            transform.position = new Vector3(posX, posY, posZ);
        }
        else
        {
            transform.position = newCameraPosition;
        }

        _lastTargetPosition = _target.position;
    }
}
