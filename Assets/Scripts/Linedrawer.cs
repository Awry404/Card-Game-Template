using UnityEngine;
using UnityEngine.UIElements;

public class Linedrawer : MonoBehaviour
{
    public SpeedDie parent;
    public EnemySpeedDie enemy_parent;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = GetComponentInParent<SpeedDie>();
        if (parent == null)
        {
            enemy_parent = GetComponentInParent<EnemySpeedDie>();
        }
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null && parent.selected_card != null)
        {
            startPoint = parent.transform.position;
            endPoint = parent.clash_target.transform.position;
            endPoint.z = 0;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
            if (parent.clash_target.clash_target != parent)
            {
                lineRenderer.startColor = Color.pink;
                lineRenderer.endColor = Color.pink;
            }
            else if (parent.clash_target.clash_target == parent)
            {
                lineRenderer.startColor = Color.orange;
                lineRenderer.endColor = Color.orange;
            }
        }
        else if (enemy_parent != null && enemy_parent.selected_card != null)
        {
            startPoint = enemy_parent.transform.position;
            endPoint = enemy_parent.clash_target.transform.position;
            endPoint.z = 0;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
            if (enemy_parent.clash_target.clash_target != enemy_parent)
            {
                lineRenderer.startColor = Color.pink;
                lineRenderer.endColor = Color.pink;
            }
            else if (enemy_parent.clash_target.clash_target == enemy_parent)
            {
                lineRenderer.startColor = Color.orange;
                lineRenderer.endColor = Color.orange;
            }
        }
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}
