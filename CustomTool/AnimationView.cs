using UnityEngine;
[RequireComponent(typeof(Animator))]

public class AnimationView : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string name)
    {
        Show();
        animator.Play(name);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        StopAnimation();
        this.gameObject.SetActive(false);

    }
    public void StopAnimation()
    {
        animator.StopPlayback();
    }

}
