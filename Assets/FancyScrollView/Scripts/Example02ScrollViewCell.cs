using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollViewExamples
{
    public class Example02ScrollViewCell
        : FancyScrollViewCell<Example02CellDto, Example02ScrollViewContext>
    {
        //[SerializeField]
        //Animator animator;
        [SerializeField]
        Text message;
        [SerializeField]
        Image image;
        [SerializeField]
        Button button;
        [SerializeField]
        RectTransform rectTransform;

        static readonly int scrollTriggerHash = Animator.StringToHash("scroll");
        Example02ScrollViewContext context;

        void Start()
        {
            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// コンテキストを設定します
        /// </summary>
        /// <param name="context"></param>
        public override void SetContext(Example02ScrollViewContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(Example02CellDto itemData)
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

        private int cellWidth = 200;
        private int scrollSpace = 30;
        /// <summary>
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
        public override void UpdatePosition(float position)
        {
            Debug.Log("UpdatePosition " + position);
            Vector3 anchorPos = rectTransform.anchoredPosition3D;
            anchorPos.x = position;
            anchorPos.z = 0;
            rectTransform.anchoredPosition3D = anchorPos;
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
