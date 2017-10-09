using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollViewExamples
{
    public class Example04ScrollViewCell
        : FancyScrollViewCell<Example04CellDto, Example04ScrollViewContext>
    {
        [SerializeField]
        Text message;
        [SerializeField]
        Image image;
        [SerializeField]
        Button button;
        [SerializeField]
        RectTransform rectTransform;

        static readonly int scrollTriggerHash = Animator.StringToHash("scroll");
        Example04ScrollViewContext context;

        void Start()
        {
            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// コンテキストを設定します
        /// </summary>
        /// <param name="context"></param>
        public override void SetContext(Example04ScrollViewContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(Example04CellDto itemData)
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
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
        public override float UpdatePosition(Vector3 preItemAnchorPos, float preItemWeight, float offset)
        {
            Vector3 anchorPos = rectTransform.anchoredPosition3D;
            //if (this.message.text.Equals("Cell 0")) {
            //    Debug.LogError(preItemAnchorPos.x + " preItemWeight " + preItemWeight);
            //}
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
