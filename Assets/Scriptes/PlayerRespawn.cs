using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip chekpointsound;
    private Transform currentChekpoint;
    private Health playerHealth;
    
    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }
    
    public void Respawn()
    {
        transform.position = currentChekpoint.position;
        playerHealth.Respawn();
        
        Camera.main.GetComponent<CameraController>().MoveToNewBuilding(currentChekpoint.parent);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {    
        if (collision.transform.tag == "Chekpoint")
        { 
            currentChekpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }   
}
