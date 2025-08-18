using System;
using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.1f;

    private void OnEnable()
    {
        DisableSelf();
    }

    private IEnumerator DisableSelf()
    {
        yield return new WaitForSeconds(flashDuration);
        gameObject.SetActive(false);
        yield return null;
    }
}
