using System.Collections;
using UnityEngine;

public class ShouchongButtonController : MonoBehaviour
{
    public Animator animator; // ����Animator���
    public float animationInterval = 4.0f; // �������ż��ʱ��

    private IEnumerator PlayAnimationPeriodically()
    {
        // ����ѭ����ֱ��Э�̱�ֹͣ
        while (true)
        {
            // �ȴ�ֱ���´β���ʱ��
            yield return new WaitForSeconds(animationInterval);

            // ���Animator����Ƿ����
            if (animator != null)
            {
                // ���Ŷ���
                
                animator.SetBool("toJump",true);
            }
        }
    }

    void Start()
    {
        // ��Start������Э��
        StartCoroutine(PlayAnimationPeriodically());
    }
    public void JumpEndEvent()
    {
        animator.SetBool("toJump", false);
    }
}