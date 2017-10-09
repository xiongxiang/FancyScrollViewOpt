using System;

namespace FancyScrollViewExamples
{
    public class DemoScrollViewContext
    {
        int selectedIndex = -1;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                var prevSelectedIndex = selectedIndex;
                selectedIndex = value;
                if (prevSelectedIndex != selectedIndex)
                {
                    if (OnSelectedIndexChanged != null)
                    {
                        OnSelectedIndexChanged(selectedIndex);
                    }
                }
            }
        }

        public Action<DemoScrollViewCell> OnPressedCell;
        public Action<int> OnSelectedIndexChanged;
    }
}
