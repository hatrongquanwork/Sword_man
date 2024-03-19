using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    private EnemySpawner enemySpawner;

    public int health = 3;
    public AudioClip soundDeath;

    private void Start()
    {
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    public void takeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Invoke("PlayDeathSoundAndDestroy", 0.1f); // Gọi phương thức PlayDeathSoundAndDestroy() sau 5 giây
        }
    }

    public void PlayDeathSoundAndDestroy()
    {
        
        AudioSource.PlayClipAtPoint(soundDeath, transform.position); // Phát âm thanh tại vị trí của đối tượng
        Destroy(this.gameObject);
        enemySpawner.EnemyDied(gameObject);
    }


}
