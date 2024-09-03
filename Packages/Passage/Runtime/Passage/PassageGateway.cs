using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]
public class PassageGateway : MonoBehaviour
{
    public Passage passage;

    public void Start()
    {
        if (passage.Name == PassageStorage.GetTarget)
        {
            TransportPosition();
        }
    }

    public void TransportPosition()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player.position = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                PassageManager.instance.CallSceneTransition(passage);
                PassageStorage.SetTarget(passage.TargetName);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(new Vector3(transform.position.x + col.offset.x, transform.position.y + col.offset.y, 0), new Vector3(col.size.x, col.size.y, 0));
    }
}