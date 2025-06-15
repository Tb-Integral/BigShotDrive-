using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image bg;
    public float speedOfFade = 5;
    private float _time = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            StartCoroutine(StartFadeOut());
        }
    }

    private IEnumerator StartFadeOut()
    {
        Color _color = bg.color;

        while (_color.a < 1f)
        {
            _color.a += speedOfFade * Time.deltaTime;
            _color.a = Mathf.Clamp01(_color.a);
            bg.color = _color;

            yield return null;
        }
    }
}
