using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private string text;

    // Método que se llamará cuando termine la animación
    public void Animation()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(AnimationCoroutine());
    }

    public IEnumerator AnimationCoroutine()
    {
        gameObject.GetComponent<Animator>().SetTrigger(text);
        yield return new WaitForSeconds(animationDuration);
        this.gameObject.SetActive(false);
    }
}
