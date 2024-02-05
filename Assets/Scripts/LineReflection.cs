using UnityEngine;

public class LineReflection : MonoBehaviour
{
    public int reflections;
    public float maxLenght;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private bool isFound = false;

    private void Update()
    {
        ray = new Ray(transform.position, transform.up);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remaningLength = maxLenght;

        for (int i = 0; i < reflections; i++)
        {
            if (!isFound)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, remaningLength))
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remaningLength -= Vector3.Distance(ray.direction, hit.normal);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }
                else
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remaningLength);
                }
            }  
            
        }
    }

}