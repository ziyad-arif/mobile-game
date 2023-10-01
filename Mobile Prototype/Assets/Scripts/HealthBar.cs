using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Transform target;
    private Vector3 offset = new Vector3(0, -0.6f, 0);
    // Update is called once per frame
    void Update()
    {
        fill.color = gradient.Evaluate(slider.value);
        if (target != null)
        {
            transform.SetPositionAndRotation(target.position + offset, Camera.main.transform.rotation);
        }       
    }
}
