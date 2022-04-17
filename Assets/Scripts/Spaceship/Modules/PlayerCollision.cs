using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerCollision : MonoBehaviour
{
    List<string> animations = new List<string>
    {
        "isShooting",
        "isMoving",
        "isIdle"
    };

    Animator animator;

    void StopAnimations()
    {
        animator = GetComponent<Animator>();

        foreach (var anim in animations)
        {
            animator.SetBool(anim, false);
        }
    }

    async void Death()
    {
        StopAnimations();
        animator.SetBool("isDead", true);
        Time.timeScale = 1f;
        await Task.Delay(600); //600 ms to play animation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var tag = collision.gameObject.tag;
        Debug.Log($"Collision with object \"{tag}\"");
        Death();
    }
}
