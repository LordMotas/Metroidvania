using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour {


    private Transform bar;

	// Use this for initialization
	private void Start () {
        bar = transform.Find("Bar");
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor(Color color)
    {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = color;
    }

}
