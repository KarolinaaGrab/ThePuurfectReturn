using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Collectible : MonoBehaviour
{
    private AudioSource audioSource;
    private CapsuleCollider capsuleCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();

            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.AddScore(1);

            capsuleCollider.enabled = false;
            meshRenderer.enabled = false;
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
