using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageInfoText : MonoBehaviour
{
    public float SpeedToGoUp = 3f;
    public float LifeTime = 2f;

    [Title("Display")]
    public TextMeshProUGUI display;

    private float _damageToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * SpeedToGoUp * Time.deltaTime);
    }

    public void SetDamage(float damage)
    {
        display.text = damage.ToString();
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
