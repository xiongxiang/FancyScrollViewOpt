using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollViewExamples
{
    public class DemoScrollView : FancyScrollView<DemoCellDto, DemoScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;
        [SerializeField]
        float scrollToDuration = 0.4f;

        protected override void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            scrollPositionController.OnItemSelected(HandleItemSelected);
            base.Awake();
        }

        public void UpdateData(List<DemoCellDto> data, DemoScrollViewContext context)
        {
            context.OnPressedCell = OnPressedCell;
            SetContext(context);

            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        public void UpdateSelection(int selectedCellIndex)
        {
            scrollPositionController.ScrollTo(selectedCellIndex, scrollToDuration);
            context.SelectedIndex = selectedCellIndex;
            UpdateContents();
        }

        void HandleItemSelected(int selectedItemIndex)
        {
            context.SelectedIndex = selectedItemIndex;
            UpdateContents();
        }

        void OnPressedCell(DemoScrollViewCell cell)
        {
            UpdateSelection(cell.DataIndex);
        }
    }
}
