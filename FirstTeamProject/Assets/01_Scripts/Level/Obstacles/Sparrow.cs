using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sparrow : MonoBehaviour
{
    private void Awake()
    {
        transform.DOMove(transform.position + transform.forward * 40, 0.4f).SetEase(Ease.OutSine)
            .OnComplete(() => { Destroy(gameObject); });
    }
}
