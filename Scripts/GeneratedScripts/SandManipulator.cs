using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class SandManipulator : MonoBehaviour
{
    public float digDepth = 0.1f;
    public float radius = 1f;
    Mesh mesh;
    Vector3[] vertices;
    MeshCollider meshCollider;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DigAtCursor(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                DigAtCursor(touch.position);
            }
        }
    }

    void DigAtCursor(Vector3 screenPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                DigHole(hit.point);
            }
        }
    }

    void DigHole(Vector3 point)
    {
        point = transform.InverseTransformPoint(point);

        for (int i = 0; i < vertices.Length; i++)
        {
            // Since we're looking from the Y-axis, we consider the X and Z positions for distance calculation.
            Vector3 tempPoint = new Vector3(vertices[i].x, 0, vertices[i].z);
            Vector3 targetPoint = new Vector3(point.x, 0, point.z);
            float distance = Vector3.Distance(targetPoint, tempPoint);

            if (distance < radius)
            {
                vertices[i].y -= digDepth * (1 - (distance / radius));
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}