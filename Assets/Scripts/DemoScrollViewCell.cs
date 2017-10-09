using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollViewExamples
{
    public class DemoScrollViewCell
        : FancyScrollViewCell<DemoCellDto, DemoScrollViewContext>
    {
        [SerializeField]
        Text message;
        [SerializeField]
        Image image;
        [SerializeField]
        Button button;
        [SerializeField]
        RectTransform rectTransform;
        
        DemoScrollViewContext context;

        void Start()
        {
            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// Set the context
        /// </summary>
        /// <param name="context"></param>
        public override void SetContext(DemoScrollViewContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Update cell contents
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(DemoCellDto itemData)
        {
            message.text = itemData.Message;

            if (context != null)
            {
                var isSelected = context.SelectedIndex == DataIndex;
                image.color = isSelected
                    ? new Color32(0, 255, 255, 100)
                    : new Color32(255, 255, 255, 77);
            }
        }

        /// <summary>
        /// Update cell position
        /// </summary>
        /// <param name="position"></param>
        public override float UpdatePosition(Vector3 preItemAnchorPos, float preItemWeight, float offset)
        {
            Vector3 anchorPos = rectTransform.anchoredPosition3D;
            anchorPos.x = preItemAnchorPos.x + preItemWeight + offset;
            anchorPos.z = 0;
            rectTransform.anchoredPosition3D = anchorPos;

            return 0f;
        }

        public override float GetItemWidth() {
            var rectTransform = transform as RectTransform;
            return rectTransform.rect.width;
        }


        public override Vector3 GetItemAnchorPos() {
            var rectTransform = transform as RectTransform;
            return rectTransform.anchoredPosition3D;
        }

        void OnPressedCell()
        {
            if (context != null)
            {
                context.OnPressedCell(this);
            }
        }
    }
}
