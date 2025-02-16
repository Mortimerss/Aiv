using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWithMouse : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private float depth; // Profondità lungo l'asse Z
    private float depthSpeed = 1f; // Velocità di movimento in profondità
    private bool isDragging = false; // Flag per il drag

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(position);

        GetComponent<Rigidbody>().isKinematic = true;
        isDragging = true;

        depth = transform.position.z; // Salviamo la profondità attuale
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = new Vector3(curPosition.x, curPosition.y, depth);

        // Permettiamo lo spostamento in profondità con la rotella del mouse **solo durante il drag**
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        depth += scroll * depthSpeed;
        depth = Mathf.Clamp(depth, -5f, 5f); // Limiti di profondità per evitare spostamenti infiniti
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        isDragging = false;
    }
}