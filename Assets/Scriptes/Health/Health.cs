using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    
    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    
    [Header ("iFrames")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;
       
   private void Awake()
   {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
   }
   
   public void TakeDamage(float _damage)
   {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
            
                foreach (Behaviour component in components)
                    component.enabled = false;
                    
                    anim.SetBool("grounded", true);
                    anim.SetTrigger("die");
                
                dead = true;
                
                
                //Player
                if(GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;
                
                //Enemy
                if(GetComponent<EnemyPatrol>() != null)
                    GetComponent<EnemyPatrol>().enabled = false;
                    
                if(GetComponent<Enemy>() != null)    
                    GetComponent<Enemy>().enabled = false;
                
                dead = true;
            }
        
        }
   }
   
   private void Update()
   {
        if(Input.GetKeyDown(KeyCode.E));
            TakeDamage(1);
   }
   
   public void AddHealth(float _value)
   {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
   }
   
   public void Respawn()
   {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
         StartCoroutine(Invunerability());
        
        foreach (Behaviour component in components)
            component.enabled = true;
   }
   
   private IEnumerator Invunerability()
   {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
   }
}


