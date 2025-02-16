using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int points;
    public int Points { get => points; set => points = value; }

    private bool isMerging = false; // Per evitare fusioni duplicate
    private const float maxSize = 2.5f; // Limite massimo di grandezza

    public AudioClip mergeSound; // Aggiunto campo per il suono
    public AudioSource audioSource; // Per riprodurre il suono

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Aggiunge un AudioSource al blocco
        if (mergeSound != null)
        {
            audioSource.clip = mergeSound;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isMerging) return; // Se è già in fusione, ignora

        Block otherBlock = collision.gameObject.GetComponent<Block>();

        if (otherBlock != null && otherBlock.Points == this.Points && !otherBlock.isMerging)
        {
            StartCoroutine(MergeBlocks(this, otherBlock));
        }
    }

    private IEnumerator MergeBlocks(Block blockA, Block blockB)
    {
        // Evitiamo che si uniscano più volte
        blockA.isMerging = true;
        blockB.isMerging = true;

        // Se il blocco è troppo grande, non si fonde più
        if (blockA.transform.localScale.x >= maxSize || blockB.transform.localScale.x >= maxSize) yield break;

        // Nuova posizione media
        Vector3 newPosition = (blockA.transform.position + blockB.transform.position) * 0.5f;

        // Creazione nuovo blocco
        GameObject newBlock = Instantiate(blockA.gameObject, newPosition, Quaternion.identity);
        Block newBlockScript = newBlock.GetComponent<Block>();

        // Aggiorna i punti e la scala
        newBlockScript.Points = blockA.Points * 2;
        newBlock.transform.localScale = blockA.transform.localScale * 1.2f;
        newBlock.transform.localRotation = Random.rotation;

        // Cambia colore 
        newBlock.GetComponent<Renderer>().material.color = Color.Lerp(
            blockA.GetComponent<Renderer>().material.color,
            blockB.GetComponent<Renderer>().material.color,
            0.5f
        );

        // Riproduce il suono della fusione
        if (newBlockScript.mergeSound != null)
        {
            newBlockScript.audioSource.PlayOneShot(newBlockScript.mergeSound);
        }

        // Disattiva la fusione per il nuovo blocco
        newBlockScript.isMerging = false;

        // Distruggi i blocchi originali
        Destroy(blockA.gameObject);
        Destroy(blockB.gameObject);
    }
}
