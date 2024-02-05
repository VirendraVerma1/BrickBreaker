using System.Collections.Generic;
using UnityEngine;

public class BallDetails : MonoBehaviour
{
    public enum MyColourCode
    {
        Red,
        Yellow,
        Blue,
        Green
    }

    public MyColourCode colorCode;

    void Start()
    {
        // Get the material name from the enum
        string materialName = colorCode.ToString();

        // Load the material from the Resources/Materials folder
        Material material = Resources.Load<Material>("Materials/" + materialName);

        if (material != null)
        {
            // Assign the material to this GameObject's Renderer
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }
            else
            {
                Debug.LogError("Renderer not found on the GameObject.");
            }
        }
        else
        {
            Debug.LogError("Material with name " + materialName + " not found in Resources/Materials.");
        }

        CheckAndInitializeNeighbour();
    }

    public BallDetails[] _neighbourBalls;

    public void CheckAndInitializeNeighbour()
    {
        float radius = 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<BallDetails> neighbourBalls = new List<BallDetails>();

        foreach (var hitCollider in hitColliders)
        {
            var ball = hitCollider.gameObject;
            var ballDetails = ball.GetComponent<BallDetails>();

            if (ballDetails != null && ballDetails.colorCode == colorCode && ball!=gameObject)
            {
                neighbourBalls.Add(ball.GetComponent<BallDetails>());
            }
        }

        _neighbourBalls = neighbourBalls.ToArray();
    }

    // Draw Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}