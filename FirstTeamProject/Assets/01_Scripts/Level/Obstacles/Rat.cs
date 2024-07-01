using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rat : MonoBehaviour
{
    private void Awake()
    {
        transform.DOMove(transform.position + transform.forward * 100, 3f)
            .OnComplete(() => { Destroy(gameObject); });
    }
}
