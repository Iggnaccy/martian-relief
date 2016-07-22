using UnityEngine;

public class BombPickupBehaviour : MonoBehaviour
{
	float scale = 1.0f;
	float scaleSpeed = 0.5f;
	float scaleX, scaleY, scaleZ;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BasicStats>().bombs++;
			Static.removeDrop(GetComponent<IDScript>().id);
            Destroy(gameObject);
        }
    }

	void Start(){
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		scaleZ = transform.localScale.z;
	}
	
	void Update(){
		scale += scaleSpeed * Time.deltaTime;
		if (scale > 1.15f) {
			scaleSpeed *= -1.0f;
			scale = 1.13f;
		}
		if (scale < 0.85f) {
			scaleSpeed *= -1.0f;
			scale = 0.87f;
		}
		transform.localScale = new Vector3 (scaleX*scale, scaleY*scale, scaleZ);
	}
}
