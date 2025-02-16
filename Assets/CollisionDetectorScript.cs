using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    int collisions = 0;
    int points = 0;

    private void OnCollisionEnter(Collision collision)
    {
        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            points += block.Points;
        }

        collisions++;
        Debug.Log($"Collisions: {collisions} | Points: {points}");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piattaforma")) return;

        Block block = collision.gameObject.GetComponent<Block>();
        if (block != null)
        {
            points -= block.Points;
        }

        collisions--;
        Debug.Log($"Collisions: {collisions} | Points: {points}");
    }
}