using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool shoot = false;
    private PlayerBrain.Direction dir;
    public float arrowSpeed = 10f;
    private float destroyTime = 1.2f;
    public PlayerBrain.Color color = PlayerBrain.Color.Blue;

    [HideInInspector] public GameObject master;
    public int ownerID = 0;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3();
        movement.x = 1;
        movement *= arrowSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == master ||
            (col.CompareTag("Player") && col.GetComponent<PlayerBrain>().color == color))
        {
            return;
        }
        Destroy(gameObject);
    }

}
