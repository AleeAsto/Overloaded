using UnityEngine;
using UnityEngine.UI;
public class ProgressBarTask : MonoBehaviour
{
    public Image fill;
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    private Transform target;

    private void Start()
    {
        transform.position = target.position + offset;
    }
    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetProgress(float value)
    {
        if (fill != null)
            fill.fillAmount = Mathf.Clamp01(value);
    }

    void LateUpdate()
    {


        // Seguir al objeto
        
    }
}
