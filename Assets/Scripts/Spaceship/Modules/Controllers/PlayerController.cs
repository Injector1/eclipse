using UnityEngine;


public class PlayerController : MonoBehaviour, ISpaceshipModule, IController
{
    private Spaceship spaceship;
    private SavedInput<float> horizontalAxis;
    private SavedInput<float> verticalAxis;
    private SavedInput<Vector3> mousePosition;

    public void Awake()
    {
        spaceship = GetComponent<Spaceship>();
        horizontalAxis = new SavedInput<float>();
        verticalAxis = new SavedInput<float>();
        mousePosition = new SavedInput<Vector3>();
    }

    public void Start()
    {
        spaceship.AddModule(this, typeof(IController));
    }

    public void Update()
    {
        if (Input.GetButton("Fire1"))       mousePosition.Value = Input.mousePosition;
        if (Input.GetButton("Horizontal"))  horizontalAxis.Value = Input.GetAxis("Horizontal");
        if (Input.GetButton("Vertical"))    verticalAxis.Value = Input.GetAxis("Vertical");
    }

    public void FixedUpdate()
    {
        if (mousePosition.IsUpdated)
        {
            var lookVector = mousePosition.Value - Camera.main.WorldToScreenPoint(spaceship.transform.position);
            spaceship.OnShoot?.Invoke(lookVector);
        }

        if (horizontalAxis.IsUpdated) 
            spaceship.OnRotate?.Invoke(horizontalAxis.Value);
        
        if (verticalAxis.IsUpdated)
        {
            if (verticalAxis.Value > 0) spaceship.OnBoost?.Invoke(verticalAxis.Value);
            else spaceship.OnSlowDown?.Invoke(verticalAxis.Value);
        }
    }
}