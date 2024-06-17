using UnityEngine;

public class CameraController : MonoBehaviour
{
      private float speed;
    private float currentPosx;
    private Vector3 velocity = Vector3.zero;
    
    //Follw player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;
    
    Vector2 Chekpoint;
    Rigidbody2D collision;
    
    
    private void update()
    {
        //Building camera
        transform.position = Vector3.SmoothDamp (transform.position, new Vector3(currentPosx, transform.position.y, transform.position.z), ref velocity, speed);
        
        //Follow player
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }
    
    public void MoveToNewBuilding(Transform _newBuilding)
    {
        currentPosx = _newBuilding.position.x;
    }
}
