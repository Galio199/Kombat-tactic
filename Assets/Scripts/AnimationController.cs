using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private string text;

    // M�todo que se llamar� cuando termine la animaci�n
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
