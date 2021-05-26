using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class PortalEffect : MonoBehaviour
    {
        public float radius;

        public float rotateSpeed;

        public int fragCounts;
        public int circleCounts;

        public float circleSize;
        public float fragSize;

        public float circleAbsorbSpeed;
        public float fragAbsorbSpeed;

        public Color circleColorOutter;
        public Color circleColorInner;

        public Color fragColorOutter;
        public Color fragColorInner;

        private Transform[] circles;
        private Transform[] frags;
        private SpriteRenderer[] circleSprites;
        private SpriteRenderer[] fragSprites;

        private void Awake()
        {
            GameObject rectPrefab = ResourceLoader.Load("Prefabs/Particles/RectPrefab") as GameObject;

            circles = new Transform[fragCounts];
            circleSprites = new SpriteRenderer[fragCounts];

            for (int i = 0; i < circleCounts; i++)
            {
                circles[i] = Pool.instance.Instantiate(rectPrefab, transform).transform;
                circles[i].localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                circles[i].localScale = (i / (float)circleCounts) * circleSize * Vector3.one;
                circleSprites[i] = circles[i].GetComponent<SpriteRenderer>();
            }

            frags = new Transform[fragCounts];
            fragSprites = new SpriteRenderer[fragCounts];

            for (int i = 0; i < fragCounts; i++)
            {
                frags[i] = Pool.instance.Instantiate(rectPrefab, transform).transform;
                frags[i].localPosition = (i / (float)fragCounts) * Random.insideUnitCircle.normalized * radius;
                frags[i].localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                fragSprites[i] = frags[i].GetComponent<SpriteRenderer>();
            }
        }

        internal virtual void Update()
        {
            Fragment();
        }

        private void Fragment()
        {
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

            for (int i = 0; i < circleCounts; i++)
            {
                float circleScale = circles[i].localScale.x;

                circleScale -= circleAbsorbSpeed * Time.deltaTime;

                if (circleScale <= 0)
                {
                    circleScale = circleSize;
                    circles[i].localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                }

                circles[i].localScale = circleScale * Vector3.one;
                circleSprites[i].color = circleColorOutter + (circleColorInner - circleColorOutter) * Mathf.Clamp(Mathf.Pow(1 - circleScale / circleSize, 2), 0, 1);
            }

            for (int i = 0; i < fragCounts; i++)
            {
                float distance = frags[i].localPosition.magnitude;

                distance -= fragAbsorbSpeed * Time.deltaTime;

                if (distance <= 0)
                {
                    frags[i].localScale = Vector3.zero;
                    frags[i].localPosition = (Random.insideUnitCircle).normalized * radius;
                    frags[i].localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                    distance = radius;
                }
                else
                {
                    frags[i].localPosition = frags[i].localPosition.normalized * distance;
                }

                float distanceRatio = distance / radius;

                frags[i].localScale = Vector3.one * Mathf.Clamp((1 - distanceRatio), 0, 1) * fragSize;

                fragSprites[i].color = fragColorOutter + (fragColorInner - fragColorOutter) * Mathf.Clamp(Mathf.Pow(1 - distanceRatio, 3), 0, 1);

            }
        }
    }
}