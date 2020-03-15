using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public float defaultAnimationTime;

    private class _TransformAnimation
    {
        private Transform transform;
        private Vector3 from;
        private Vector3 to;
        private float animTime;
        private float interpolant;

        public bool IsCompleted()
        {
            return interpolant >= 1.0f;
        }

        public void ExecuteStep(float deltaTime)
        {
            float deltaInterpolant = deltaTime / animTime;
            interpolant = Mathf.Min(1.0f, interpolant + deltaInterpolant);

            transform.position = Vector3.Lerp(from, to, interpolant);
        }

        private void Init(Transform transform, Vector3 from, Vector3 to, float animTime)
        {
            this.transform = transform;
            this.from = from;
            this.to = to;
            this.animTime = animTime;
        }
        public _TransformAnimation(Transform transform, Vector3 from, Vector3 to, float animTime)
        {
            Init(transform, from, to, animTime);
        }
    }

    private List<_TransformAnimation> transformAnimations;

    public void AnimateTransformPos(Transform transform, Vector3 from, Vector3 to, float animTime)
    {
        _TransformAnimation anim = new _TransformAnimation(transform, from, to, animTime);
        transformAnimations.Add(anim);
    }
    public void AnimateTransformPos(Transform transform, Vector3 from, Vector3 to)
    {
        AnimateTransformPos(transform, from, to, defaultAnimationTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        transformAnimations = new List<_TransformAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transformAnimations.Count; i++)
        {
            _TransformAnimation anim = transformAnimations[i];
            anim.ExecuteStep(Time.deltaTime);
            if (anim.IsCompleted())
            {
                transformAnimations.RemoveAt(i);
                i -= 1;
            }
        }
    }
}
