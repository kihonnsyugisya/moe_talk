using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarView : MonoBehaviour
{
    [SerializeField,Range(0,1f)] private float weight;
    // Start is called before the first frame update
    void Start()
    {
        //animator.Play("MTH_E");

    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetLayerWeight(1, weight);
    }

    public Animator animator;
    public AnimationClip[] animations;

    public string TranslateEmoToFaceState(EMOTIONS emo)
    {
        return emo switch
        {
            EMOTIONS.HAPPY or EMOTIONS.LOVE => animations[1].name,
            EMOTIONS.SAD => animations[2].name,
            EMOTIONS.FEAR => animations[6].name,
            EMOTIONS.ANGRY => animations[3].name,
            _ => animations[0].name,
        };
    }

    public void ResetFace()
    {
        animator.SetLayerWeight(1, 0f);
    }

    public void PlayAnimation(EMOTIONS emo,int value)
    {
        animator.SetLayerWeight(1, value / 10f);
        animator.Play(TranslateEmoToFaceState(emo));
        animator.SetTrigger(emo.ToString());
    }

    public void FortuneEmotion()
    {
        int fortune = Random.Range(0, 10);
        if (fortune > System.Enum.GetNames(typeof(EMOTIONS)).Length) return;
        EMOTIONS emo = (EMOTIONS)fortune;
        animator.SetLayerWeight(1, Random.Range(0f, 1.1f));
        animator.Play(TranslateEmoToFaceState(emo));
        animator.SetTrigger(emo.ToString());
    }

}
