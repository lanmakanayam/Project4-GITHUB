using UnityEngine;

public class NextBuilding : MonoBehaviour
{
    [SerializeField] private Transform previousBuilding;
    [SerializeField] private Transform nextBuilding;
    [SerializeField] private CameraController cam;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewBuilding(nextBuilding);
            else
                cam.MoveToNewBuilding(previousBuilding);
        }
    }
}
