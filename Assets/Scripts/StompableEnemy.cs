using UnityEngine;

public class StompableEnemy : MonoBehaviour
{
    public Animator animator;
    public Collider2D enemyCollider;
    public int scoreOnDeath = 200;
    public string deathBoolName = "isDead";
    public float destroyDelay = 0.8f;

    private bool isDead = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (enemyCollider == null)
            enemyCollider = GetComponent<Collider2D>();
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        GameHUDManager hud = FindFirstObjectByType<GameHUDManager>();

        if (hud != null)
        {
            hud.AddScore(scoreOnDeath);

        }
        if (animator != null)
            animator.SetBool(deathBoolName, true);

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        SlimeMovement slimeMovement = GetComponent<SlimeMovement>();
        MimeticMovement mimeticMovement = GetComponent<MimeticMovement>();

        if (slimeMovement != null)
            slimeMovement.enabled = false;

        if (mimeticMovement != null)
            mimeticMovement.enabled = false;

        Destroy(gameObject, destroyDelay);
    }
}