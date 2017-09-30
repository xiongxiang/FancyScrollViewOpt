using System.Collections.Generic;
using UnityEngine;

public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class {
    [SerializeField]
    bool loop;
    [SerializeField]
    GameObject cellBase;

    float currentPosition;
    readonly List<FancyScrollViewCell<TData, TContext>> cells =
        new List<FancyScrollViewCell<TData, TContext>>();

    protected TContext context;
    protected List<TData> cellData = new List<TData>();
    private float viewWidth = 750;
    private float viewHeight = 1335;

    protected virtual void Awake() {
        cellBase.SetActive(false);
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
        var cellObject = Instantiate(cellBase);
        cellObject.SetActive(true);
        var cell = cellObject.GetComponent<FancyScrollViewCell<TData, TContext>>();
        cell.transform.SetParent(cellBase.transform.parent);
        cell.transform.localScale = Vector3.one;
        cell.transform.localPosition = Vector3.zero;
        cell.SetContext(context);
        cell.SetVisible(false);

        return cell;
    }

    /// <summary>
    /// セルの内容を更新します
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="dataIndex"></param>
    void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex) {
        if (loop) {
            dataIndex = GetLoopIndex(dataIndex, cellData.Count);
        } else if (dataIndex < 0 || dataIndex > cellData.Count - 1) {
            // セルに対応するデータが存在しなければセルを表示しない
            cell.SetVisible(false);
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


    private float cellInterval = 0.23f;
    private float prePosition;
    private float curPosition;
    /// <summary>
    /// スクロール位置を更新します
    /// </summary>
    /// <param name="position"></param>
    protected void UpdatePosition(float position) {
        currentPosition = position;
        Debug.LogError(position);

        var firstCellPosition = Mathf.Ceil(position) - position;
        var dataStartIndex = Mathf.CeilToInt(position);
        var count = 0;
        var cellIndex = 0;

        for (float pos = firstCellPosition; pos <= 1f; pos += cellInterval, count++) {
            if (count >= cells.Count) {
                cells.Add(CreateCell());
            }
        }


        count = 0;
        for (float pos = firstCellPosition; pos <= 1f; count++, pos += cellInterval) {
            var dataIndex = dataStartIndex + count;
            cellIndex = GetLoopIndex(dataIndex, cells.Count);
            if (cells[cellIndex].gameObject.activeSelf) {
                if (count == 1) {
                    prePosition = this.viewWidth * pos;
                    curPosition = prePosition;
                } else {
                    curPosition = prePosition + 200 + 10;
                }
                cells[cellIndex].UpdatePosition(curPosition);
                prePosition = curPosition;
            }
            UpdateCellForIndex(cells[cellIndex], dataIndex);
        }

        cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count);

        for (; count < cells.Count; count++, cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count)) {
            cells[cellIndex].SetVisible(false);
        }
    }
}

public sealed class FancyScrollViewNullContext {

}

public class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext> {

}
