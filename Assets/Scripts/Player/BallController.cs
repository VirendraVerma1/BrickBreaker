using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody ballRigidbody; // Reference to the ball's Rigidbody
    public float forceMultiplier = 10.0f; // Multiplier for the force to make it more or less powerful
    public float rotationSpeed = 100.0f; // Speed of rotation

    private Camera mainCamera; // Reference to the main camera
    private bool isRotating = false; // Flag to check if rotation should occur

    public delegate void OnStickModeChanged();
    public static event OnStickModeChanged onStickModeChanged;
    
    public enum BallMode
    {
        Idle,
        Aiming,
        Shoot,
        Stick,
        Disable
    }

    private BallMode _ballMode = BallMode.Idle;

    public BallMode _BallMode
    {
        get { return _ballMode; }
        set
        {
            _ballMode = value;
            switch (_ballMode)
            {
                case BallMode.Idle:
                    SetUpIdleMode();
                    break;
                case BallMode.Aiming:
                    SetUpAimingMode();
                    break;
                case BallMode.Shoot:
                    SetUpShootMode();
                    break;
                case BallMode.Disable:
                    SetUpDisableMode();
                    break;
                case BallMode.Stick:
                    SetUpStickMode();
                    break;
                    
            }
        }
    }
    
    void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        // Ensure there's a Rigidbody component attached to the ball
        if (ballRigidbody == null)
        {
            ballRigidbody = GetComponent<Rigidbody>();
        }

        _BallMode = BallMode.Idle;
    }

    private bool isBallSpawn = false;
    void Update()
    {
        RotateObjectTowardsMouse();

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _BallMode = BallMode.Shoot;
            isRotating = false;

            // Apply force towards the mouse position in the X and Y directions
            if(!isBallSpawn)
                ApplyForceTowardsMouse();
            _BallMode = BallMode.Idle;
        }

        // Check if the left mouse button is held down
        if (Input.GetMouseButtonDown(0))
        {
            _BallMode = BallMode.Aiming;
            isRotating = true;
        }
    }

    private void RotateObjectTowardsMouse()
    {
        if (isRotating)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(transform.position).z;

            Vector3 objectPos = mainCamera.ScreenToWorldPoint(mousePos);
            Vector3 direction = objectPos - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyForceTowardsMouse()
    {
        isBallSpawn = true;
        // Convert the mouse position to a ray
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Calculate a point in 3D space where the ray intersects the y-plane of the ball
        float enter = 0.0f;
        if (new Plane(Vector3.forward, transform.position).Raycast(ray, out enter))
        {
            // Determine the point where the ray intersects the plane
            Vector3 hitPoint = ray.GetPoint(enter);

            // Calculate the direction from the ball to the hit point
            Vector3 direction = new Vector3(hitPoint.x - transform.position.x, hitPoint.y - transform.position.y, 0);

            // Apply the force to the ball
            ballRigidbody.AddForce(direction.normalized * forceMultiplier, ForceMode.Impulse);
            StartCoroutine(SpawnMoreBalls(direction));
        }
    }

    public GameObject Balls;
    public Transform _GameController;
    IEnumerator SpawnMoreBalls(Vector3 direction)
    {
        _GameController = GameObject.FindWithTag("GameController").transform;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject g = Instantiate(Balls, _GameController.transform.position, gameObject.transform.rotation);
            g.GetComponent<Rigidbody>().AddForce(direction.normalized * forceMultiplier, ForceMode.Impulse);
        }
    }
    
    #region Modes SetUp
    
    bool isShooting=false;
    public LineReflection _lineReflection;
    public LineRenderer _lineRenderer;
    
    
    #region Idle Mode

    void SetUpIdleMode()
    {
        _lineReflection.enabled = false;
        _lineRenderer.enabled = false;
        isShooting = false;
    }
    
    #endregion
    
    #region Aiming Mode

    void SetUpAimingMode()
    {
        _lineReflection.enabled = true;
        _lineRenderer.enabled = true;
        isShooting = false;
    }
    
    #endregion
    
    #region Shoot Mode

    void SetUpShootMode()
    {
        _lineReflection.enabled = false;
        _lineRenderer.enabled = false;
        isShooting = true;
    }
    
    #endregion
    
    #region Disable Mode

    void SetUpDisableMode()
    {
        _lineReflection.enabled = false;
        _lineRenderer.enabled = false;
        isShooting = false;
    }
    
    #endregion
    
    
    #region Stick Mode

    void SetUpStickMode()
    {
        onStickModeChanged?.Invoke();
        Destroy(_lineReflection);
        Destroy(_lineRenderer);
        Destroy(ballRigidbody);
        //Destroy(gameObject.GetComponent<BallColision>());
        Destroy(gameObject.GetComponent<BallController>());
    }
    
    #endregion
    
    
    #endregion
}
