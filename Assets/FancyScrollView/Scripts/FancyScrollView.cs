using System.Collections.Generic;
using UnityEngine;

public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class {
    [SerializeField, Range(float.Epsilon, 1f)]
    float cellInterval;
    [SerializeField, Range(0f, 1f)]
    float cellOffset;
    [SerializeField]
    bool loop;
    [SerializeField]
    RectTransform pnlList;

    float currentPosition;
    readonly List<FancyScrollViewCell<TData, TContext>> cells =
        new List<FancyScrollViewCell<TData, TContext>>();

    protected TContext context;
    protected List<TData> cellData = new List<TData>();

    protected virtual void Awake() {
    }

    /// <summary>
    /// コンテキストを設定します
    /// </summary>
    /// <param name="context"></param>
    protected void SetContext(TContext context) {
        this.context = context;

        for (int i = 0; i < cells.Count; i++) {
            cells[i].SetContext(context);
        }
    }

    /// <summary>
    /// セルを生成して返します
    /// </summary>
    /// <returns></returns>
    FancyScrollViewCell<TData, TContext> CreateCell() {
        GameObject cellBase = Resources.Load<GameObject>("Cell");
        var cellObject = Instantiate(cellBase);
        var cell = cellObject.GetComponent<FancyScrollViewCell<TData, TContext>>();

        cell.transform.SetParent(this.pnlList);
        cell.transform.localScale = Vector3.one;
        var cellRectTransform = cell.transform as RectTransform;
        cellRectTransform.anchoredPosition3D = Vector3.zero;

        cell.SetContext(context);

        return cell;
    }

#if UNITY_EDITOR
    float prevCellInterval, prevCellOffset;
    bool prevLoop;

    void LateUpdate() {
        if (prevLoop != loop ||
            prevCellOffset != cellOffset ||
            prevCellInterval != cellInterval) {
            UpdatePosition(currentPosition);

            prevLoop = loop;
            prevCellOffset = cellOffset;
            prevCellInterval = cellInterval;
        }
    }
#endif

    /// <summary>
    /// セルの内容を更新します
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="dataIndex"></param>
    void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex) {
        if (loop) {
            dataIndex = GetLoopIndex(dataIndex, cellData.Count);
        } else if (dataIndex < 0 || dataIndex > cellData.Count - 1) {
            Debug.LogError("UpdateCellForIndex " + dataIndex);
            //cell.SetVisible(false);
            return;
        }

        cell.SetVisible(true);
        cell.DataIndex = dataIndex;
        cell.UpdateContent(cellData[dataIndex]);
    }

    /// <summary>
    /// 円環構造の index を取得します
    /// </summary>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    int GetLoopIndex(int index, int length) {
        if (index < 0) {
            index = (length - 1) + (index + 1) % length;
        } else if (index > length - 1) {
            index = index % length;
        }
        return index;
    }

    /// <summary>
    /// 表示内容を更新します
    /// </summary>
    protected void UpdateContents() {
        UpdatePosition(currentPosition);
    }

    private void ClearChildren(Transform root) {
        int childCount = root.childCount;
        Transform[] removeList = new Transform[childCount];
        for (int i = 0; i < childCount; i++) {
            removeList[i] = root.GetChild(i);
        }
        for (int i = 0; i < childCount; i++) {
            GameObject.Destroy(removeList[i].gameObject);
        }
    }


    /// <summary>
    /// スクロール位置を更新します
    /// </summary>
    /// <param name="position"></param>
    protected void UpdatePosition(float position) {
        currentPosition = position;

        if (this.pnlList.childCount < 7) {
            this.ClearChildren(this.pnlList);
            for (int i = 0; i < 7; i++) {
                cells.Add(CreateCell());
            }
        }

        Vector3 preItemAnchorPos = Vector3.zero;
        preItemAnchorPos.x -= cells[0].GetItemWidth() * position;
        cells[0].UpdatePosition(preItemAnchorPos, 0, 0);
        UpdateCellForIndex(cells[0], 0);

        var dataIndex = 0;
        float itemWidth = 0;
        int cellsCount = cells.Count;
        for (int i = 1; i < cellsCount; i++) {
            //dataIndex = Mathf.CeilToInt(position) + i;
            //int cellIndex = GetLoopIndex(dataIndex, cellsCount);
            dataIndex = i;
            int cellIndex = i;
            if (cells[cellIndex].gameObject.activeSelf) {
                preItemAnchorPos = cells[cellIndex - 1].GetItemAnchorPos();
                itemWidth = cells[cellIndex - 1].GetItemWidth();
                cells[cellIndex].UpdatePosition(preItemAnchorPos, itemWidth, 10);
            }
            UpdateCellForIndex(cells[cellIndex], dataIndex);
        }

        //cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count);

        //for (; count < cells.Count; count++, cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count))
        //{
        //    cells[cellIndex].SetVisible(false);
        //}
    }
}

public sealed class FancyScrollViewNullContext {

}

public class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext> {

}
