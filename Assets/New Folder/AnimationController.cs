using UnityEngine;

[System.Serializable]
public class AnimationController
{
    public string animationName;
    public AnimationClip anim;
    public GameObject ObjAnim;

    public void SetUpAnimation()
    {
        Animation aniObj = ObjAnim.GetComponent<Animation>();
        aniObj.clip = anim;
        aniObj.AddClip(anim, anim.name);
    }
}
