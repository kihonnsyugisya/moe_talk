using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        animator.SetLayerWeight(1,1f);

    }

    // Update is called once per frame
    void Update()
    {
        animator.Play(animations[1].name);
    }

    public Animator animator;
    public AnimationClip[] animations;
}
