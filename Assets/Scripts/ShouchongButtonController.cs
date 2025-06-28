using System.Collections;
using UnityEngine;

public class ShouchongButtonController : MonoBehaviour
{
    public Animator animator; // 引用Animator组件
    public float animationInterval = 4.0f; // 动画播放间隔时间

    private IEnumerator PlayAnimationPeriodically()
    {
        // 无限循环，直到协程被停止
        while (true)
        {
            // 等待直到下次播放时间
            yield return new WaitForSeconds(animationInterval);

            // 检查Animator组件是否存在
            if (animator != null)
            {
                // 播放动画
                
                animator.SetBool("toJump",true);
            }
        }
    }

    void Start()
    {
        // 在Start中启动协程
        StartCoroutine(PlayAnimationPeriodically());
    }
    public void JumpEndEvent()
    {
        animator.SetBool("toJump", false);
    }
}