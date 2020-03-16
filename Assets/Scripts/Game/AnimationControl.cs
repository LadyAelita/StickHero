using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public float defaultAnimationTime;

    public delegate void AnimationCallback(Transform transform);

    private class _TransformAnimation
    {
        public enum Property { Position, RotationEuler }

        private Transform transform;
        private Vector3 from;
        private Vector3 to;
        private float animTime;
        private float interpolant;

        private AnimationCallback callback;

        private Property property;

        public bool IsCompleted()
        {
            return interpolant >= 1.0f;
        }

        public void ExecuteStep(float deltaTime)
        {
            float deltaInterpolant = deltaTime / animTime;
            interpolant = Mathf.Min(1.0f, interpolant + deltaInterpolant);

            Vector3 resultVector = Vector3.Lerp(from, to, interpolant);

            if (property == Property.Position)
            {
                transform.position = resultVector;
            }
            else if (property == Property.RotationEuler)
            {
                transform.eulerAngles = resultVector;
            }
        }

        public void HandleCompletion()
        {
            callback?.Invoke(transform);
        }

        private void Init(Transform transform, Vector3 from, Vector3 to, float animTime, Property property, AnimationCallback callback = null)
        {
            this.transform = transform;
            this.from = from;
            this.to = to;
            this.animTime = animTime;
            this.property = property;
            this.callback = callback;
        }
        public _TransformAnimation(Transform transform, Vector3 from, Vector3 to, float animTime, Property property, AnimationCallback callback = null)
        {
            Init(transform, from, to, animTime, property, callback);
        }
    }

    private List<_TransformAnimation> transformAnimations;  

    public void AnimateTransformPos(Transform transform, Vector3 from, Vector3 to, AnimationCallback callback, float animTime)
    {
        _TransformAnimation anim = new _TransformAnimation(transform, from, to, animTime, _TransformAnimation.Property.Position, callback);
        transformAnimations.Add(anim);
    }
    public void AnimateTransformPos(Transform transform, Vector3 from, Vector3 to, AnimationCallback callback = null)
    {
        AnimateTransformPos(transform, from, to, callback, defaultAnimationTime);
    }

    public void AnimateTransformEuler(Transform transform, Vector3 from, Vector3 to, AnimationCallback callback, float animTime)
    {
        _TransformAnimation anim = new _TransformAnimation(transform, from, to, animTime, _TransformAnimation.Property.RotationEuler, callback);
        transformAnimations.Add(anim);
    }
    public void AnimateTransformEuler(Transform transform, Vector3 from, Vector3 to, AnimationCallback callback = null)
    {
        AnimateTransformEuler(transform, from, to, callback, defaultAnimationTime);
    }

    // Start is called before the first frame update
    void Awake()
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
                anim.HandleCompletion();
                transformAnimations.RemoveAt(i);
                i -= 1;
            }
        }
    }
}
