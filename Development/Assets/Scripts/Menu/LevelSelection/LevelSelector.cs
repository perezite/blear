namespace Menu.LevelSelection
{
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    // level selector menu element
    // beware, this class contains a lot of hackish stuff which I do not understand properly myself
    public class LevelSelector : MenuElement
    {
        [Tooltip("Associated event system")]
        public EventSystem EventSystem;

        [Tooltip("Strength with which the buttons snap is applied")]
        public float ButtonSnapStrength = 5f;

        // parent layout group
        private GridLayoutGroup layoutGroup;

        // rect transform of the grid
        private RectTransform rectTransform;

        // parent layout group rect transform
        private RectTransform scrollRectRectTransform;

        // parent scroll rect
        private ScrollRect scrollRect;

        // canvas camera
        private Camera worldCamera;

        // is focusing last visited level
        private bool isFocusing = false;

        public override void SetInteractable(bool becomeInteractable)
        {
            if (!isFocusing)
            {
                var levelButtons = transform.GetComponentsInChildren<LevelButton>().ToList();
                levelButtons.ForEach(x => x.SetInteractable(becomeInteractable));
            }
        }

        protected override IEnumerator OnClickHandler()
        {
            yield return null;
        }

        private void Start()
        {
            layoutGroup = GetComponent<GridLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
            worldCamera = GetComponentInParent<Canvas>().worldCamera;
            scrollRectRectTransform = transform.parent.GetComponent<RectTransform>();
            scrollRect = transform.parent.GetComponent<ScrollRect>();

            // register events
            worldCamera.GetComponent<MainCamera>().CameraResolutionChanged += AdjustSize;
            GetComponent<MenuElementAnimator>().GrowAnimationCompleted += OnGrowAnimationCompleted;

            // adjust size
            AdjustSizeInternal();

            // set position
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0f);
        }

        private void OnGrowAnimationCompleted()
        {
            StartCoroutine(FocusLastVisitedLevel());
        }

        private IEnumerator FocusLastVisitedLevel()
        {
            // start focusing
            isFocusing = true;
            EventSystem.enabled = false;
            var buttons = transform.GetComponentsInChildren<LevelButton>().ToList();
            var button = buttons.Where(x => x.WasLevelVisitedByPlayer()).OrderByDescending(x => x.RelativeTargetLevelIndex).FirstOrDefault();
            var targetPosition = new Vector2(0f, transform.position.y - button.transform.position.y + 2f);
            buttons.ForEach(x => x.SetInteractable(false));

            // lerp to target
            var lerpVelocity = 6f;
            while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
            {
                if (button)
                {
                    if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
                    {
                        lerpVelocity = 25f;
                    }

                    transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * lerpVelocity);
                }

                yield return null;
            }

            // finish focusing
            transform.position = targetPosition;
            buttons.ForEach(x => x.SetInteractable(true));
            EventSystem.enabled = true;
            isFocusing = false;
        }

        private void AdjustSize()
        {
            AdjustSizeInternal();
        }

        private void AdjustSizeInternal()
        {
            // auto adjust the width of the grid to have space for all the children
            float newHeight = (transform.GetNumActiveChildren() * layoutGroup.cellSize.y) + ((transform.GetNumActiveChildren() - 1f) * layoutGroup.spacing.y);
            float extraHeight = scrollRectRectTransform.rect.height;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight + extraHeight);
        }

        private void Update()
        {
            Transform centeredButton = null;

            // mark centered button by enlarging it
            float groupSize = layoutGroup.cellSize.y + layoutGroup.spacing.y;
            float offset = (transform.GetNumActiveChildren() % 2 == 0) ? groupSize / 2f : 0f;  // offset for even number of children
            float tempPos = Mathf.Round((rectTransform.localPosition.y - offset) / groupSize) * groupSize * -1f;
            tempPos -= offset;
            var activeChildren = transform.GetActiveChildren();
            foreach (var child in activeChildren)
            {
                if (Mathf.Approximately(child.localPosition.y, tempPos))
                {
                    // enlarge
                    child.localScale = Vector3.Lerp(child.localScale, new Vector3(1.4f, 1.4f, 1f), Time.deltaTime * 15f);
                    centeredButton = child;
                }
                else
                {
                    // scale down
                    child.localScale = Vector3.Lerp(child.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 15f);
                }
            }

            // snap to center button
            if (centeredButton != null && !InputHelper.GetTap() && !isFocusing && scrollRect.velocity.magnitude < ButtonSnapStrength * 20f)
            {
                float distance = 2f - centeredButton.GetComponent<RectTransform>().position.y; // a perfectly centered button is positioned right at y = 2
                float deltaPosition = distance * ButtonSnapStrength * Time.deltaTime;
                rectTransform.position = new Vector2(rectTransform.position.x, rectTransform.position.y + deltaPosition);
            }
        }
    }
}