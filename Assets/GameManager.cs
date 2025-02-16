using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PrimitiveType primitiveToPlace;
    Vector3 nextShapePreviewPos = new Vector3(-6.73f, -0.56f, 4.84f);
    GameObject previewObject;
    int nextPoints;

    private AudioClip mergeSound; //Caricheremo questo da Resources

    private void Start()
    {
        //Carica il suono di fusione all'inizio
        mergeSound = Resources.Load<AudioClip>("merge_sound");

        GenerateNextShape();
    }

    void GenerateNextShape()
    {
        string objectTag = null;
        switch (Random.Range(0, 4))
        {
            case 0: primitiveToPlace = PrimitiveType.Cube; objectTag = "Block1"; nextPoints = 3; break;
            case 1: primitiveToPlace = PrimitiveType.Sphere; objectTag = "Block2"; nextPoints = 7; break;
            case 2: primitiveToPlace = PrimitiveType.Capsule; objectTag = "Block3"; nextPoints = 11; break;
            case 3: primitiveToPlace = PrimitiveType.Cylinder; objectTag = "Block4"; nextPoints = 13; break;
            default: primitiveToPlace = PrimitiveType.Cube; objectTag = "Block1"; nextPoints = 3; break;
        }

        if (previewObject) Destroy(previewObject);

        previewObject = GameObject.CreatePrimitive(primitiveToPlace);
        previewObject.name = "Preview shape";
        previewObject.tag = objectTag;
        previewObject.transform.position = nextShapePreviewPos;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Tasto destro del mouse
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                GameObject go = GameObject.CreatePrimitive(primitiveToPlace);
                go.tag = previewObject.tag;
                go.transform.localScale = Vector3.one * 0.3f;
                go.transform.position = hit.point + new Vector3(0, 1f, 0);
                go.transform.rotation = Random.rotation;
                go.AddComponent<Rigidbody>();

                // Assegna punti e componente Block
                Block block = go.AddComponent<Block>();
                block.Points = nextPoints;

                //Assegna il Suono
                block.mergeSound = mergeSound;

                //Aggiunge un AudioSource e assegna il suono
                AudioSource audioSource = go.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.clip = mergeSound;
                block.audioSource = audioSource; // Salva il riferimento all'AudioSource

                block.mergeSound = Resources.Load<AudioClip>("merge_sound");
                // Assegna colore casuale
                Color randomColor = Random.ColorHSV();
                float H, S, V;
                Color.RGBToHSV(randomColor, out H, out S, out V);
                S = 0.8f; V = 0.8f;
                randomColor = Color.HSVToRGB(H, S, V);
                go.GetComponent<MeshRenderer>().material.color = randomColor;

                //Assegna la texture come fatto prima
                Texture texture = Resources.Load<Texture>("wood_texture");
                go.GetComponent<MeshRenderer>().material.mainTexture = texture;

                // Aggiunge componenti extra
                go.AddComponent<DestroyOnFall>();
                go.AddComponent<DragWithMouse>();

                GenerateNextShape();
            }
        }
    }
}
