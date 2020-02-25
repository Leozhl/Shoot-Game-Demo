using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothing = 5f;

    Vector3 offset;

	void Start ()
    {
        offset = transform.position - player.position;
	}
	
	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, smoothing);
	}
}