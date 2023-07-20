using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    [SerializeField] private string text;

    private Vector3 originalPosition;
    // M�todo que se llamar� cuando termine la animaci�n
    public void Animation()
    {
        originalPosition = transform.localPosition;

        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            transform.localPosition = new Vector3(-originalPosition.x, originalPosition.y, originalPosition.z);
        }
        gameObject.SetActive(true);
        StartCoroutine(AnimationCoroutine());
    }

    public IEnumerator AnimationCoroutine()
    {
        gameObject.GetComponent<Animator>().SetTrigger(text);
        yield return new WaitForSeconds(animationDuration);
        gameObject.SetActive(false);
        transform.localPosition = originalPosition;
    }
}
