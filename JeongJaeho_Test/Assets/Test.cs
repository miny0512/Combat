using System;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]
public class Test : MonoBehaviour
{
    public Transform g1;
    public Transform g2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float angle = GetAngle2D(g1, g2);
        Debug.Log(angle);
        g1.rotation = Quaternion.Euler(0, 0, angle);
    }

    private float GetAngle2D(Transform g1, Transform g2)
    {
        Vector2 forward = g1.right;

        float x = g2.position.x - g1.position.x;
        float y = g2.position.y - g1.position.y;
        var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return angle;   
    }
    private void Rotate()
    {
        float angle = GetAngle(g1, g2);
        if (Mathf.Approximately(angle, 0)) return;

        //sg1.transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);
        // g1.transform.Rotate(new Vector3(0, angle, 0));
    }

    float Dot(Vector3 left, Vector3 right)
    {
        return (left.x * right.x) + (right.y * left.y) + (left.z * right.z);
    }

    Vector3 Cross(Vector3 left, Vector3 right)
    {
        float x = (left.y * right.z) - (left.z * right.y);
        float y = (left.z * right.x) - (left.x * right.z);
        float z = (left.x * right.y) - (left.y * right.x);
        return new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(g1.position, g1.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(g1.position, g1.right);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(g1.position, Cross(g1.forward, (g2.position - g1.position).normalized));

    }
    float GetAngle(Transform pivot, Transform target)
    {
        Vector3 targetVector = (target.position - pivot.position).normalized;
        float dot = Dot(pivot.forward, targetVector);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        var normal = Cross(g1.forward, (g2.position - g1.position).normalized);
        if (normal.y < 0f)
        {
            angle *= -1f;
        }
        return angle;
    }

}
