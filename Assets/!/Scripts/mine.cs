using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class mine : MonoBehaviour
{
    public RectTransform graphContainer;
    public Sprite dotSprite;
    public Color dotColor;
    public Color gridColor;
    public Color axisColor;
    Vector2[] dataPoints = new Vector2[1000];

    void Start()
    {
        // 生成示例数据点
        for (int i = 0; i < dataPoints.Length; i++)
        {
            dataPoints[i] = new Vector2(i, Mathf.Sin(i * 0.1f)); // 示例数据，你可以根据需要修改生成规则
        }

        DrawGrid();
        DrawAxes();
        DrawLineChart(dataPoints);
    }

    void DrawGrid()
    {
        // 绘制网格
        RectTransform grid = new GameObject("Grid", typeof(Image)).GetComponent<RectTransform>();
        grid.SetParent(graphContainer, false);
        grid.anchorMin = Vector2.zero;
        grid.anchorMax = Vector2.one;
        grid.sizeDelta = Vector2.zero;

        Image gridImage = grid.GetComponent<Image>();
        gridImage.color = gridColor;

        // 绘制横向网格线
        for (float i = 0.1f; i < 1f; i += 0.1f)
        {
            RectTransform line = new GameObject("GridLine", typeof(Image)).GetComponent<RectTransform>();
            line.SetParent(grid, false);
            line.anchorMin = new Vector2(0, i);
            line.anchorMax = new Vector2(1, i);
            line.sizeDelta = new Vector2(0, 2);
            line.anchoredPosition = Vector2.zero;
            line.GetComponent<Image>().color = gridColor;
        }

        // 绘制纵向网格线
        for (float i = 0.1f; i < 1f; i += 0.1f)
        {
            RectTransform line = new GameObject("GridLine", typeof(Image)).GetComponent<RectTransform>();
            line.SetParent(grid, false);
            line.anchorMin = new Vector2(i, 0);
            line.anchorMax = new Vector2(i, 1);
            line.sizeDelta = new Vector2(2, 0);
            line.anchoredPosition = Vector2.zero;
            line.GetComponent<Image>().color = gridColor;
        }
    }

    void DrawAxes()
    {
        // 绘制坐标轴
        RectTransform yAxis = new GameObject("YAxis", typeof(Image)).GetComponent<RectTransform>();
        yAxis.SetParent(graphContainer, false);
        yAxis.anchorMin = new Vector2(0, 0);
        yAxis.anchorMax = new Vector2(0, 1);
        yAxis.sizeDelta = new Vector2(2, 0);
        yAxis.anchoredPosition = Vector2.zero;
        yAxis.GetComponent<Image>().color = axisColor;

        RectTransform xAxis = new GameObject("XAxis", typeof(Image)).GetComponent<RectTransform>();
        xAxis.SetParent(graphContainer, false);
        xAxis.anchorMin = new Vector2(0, 0);
        xAxis.anchorMax = new Vector2(1, 0);
        xAxis.sizeDelta = new Vector2(0, 2);
        xAxis.anchoredPosition = Vector2.zero;
        xAxis.GetComponent<Image>().color = axisColor;

        // 绘制额外的格子线
        for (int i = 1; i < 100; i++)
        {
            float normalizedPosition = i / (float)100;

            // 绘制横向网格线
            RectTransform yGridLine = new GameObject("YGridLine" + i, typeof(Image)).GetComponent<RectTransform>();
            yGridLine.SetParent(graphContainer, false);
            yGridLine.anchorMin = new Vector2(0, normalizedPosition);
            yGridLine.anchorMax = new Vector2(1, normalizedPosition);
            yGridLine.sizeDelta = new Vector2(2, 0);
            yGridLine.anchoredPosition = Vector2.zero;
            yGridLine.GetComponent<Image>().color = gridColor;

            // 绘制纵向网格线
            RectTransform xGridLine = new GameObject("XGridLine" + i, typeof(Image)).GetComponent<RectTransform>();
            xGridLine.SetParent(graphContainer, false);
            xGridLine.anchorMin = new Vector2(normalizedPosition, 0);
            xGridLine.anchorMax = new Vector2(normalizedPosition, 1);
            xGridLine.sizeDelta = new Vector2(0, 2);
            xGridLine.anchoredPosition = Vector2.zero;
            xGridLine.GetComponent<Image>().color = gridColor;
        }
    }

    void DrawLineChart(Vector2[] dataPoints)
    {
        for (int i = 0; i < dataPoints.Length; i++)
        {
            // 创建数据点的UI Image
            RectTransform dot = CreateDot(dataPoints[i]);
            dot.SetParent(graphContainer, false);

            // 连接数据点之间的线
            if (i > 0)
            {
                DrawLineBetweenDots(dataPoints[i - 1], dataPoints[i]);
            }
        }
    }

    RectTransform CreateDot(Vector2 anchoredPosition)
    {
        // 创建UI Image作为数据点
        GameObject dotGameObject = new GameObject("Dot", typeof(Image));
        dotGameObject.transform.SetParent(graphContainer, false);

        RectTransform dotRectTransform = dotGameObject.GetComponent<RectTransform>();
        dotRectTransform.anchoredPosition = anchoredPosition;
        dotRectTransform.sizeDelta = new Vector2(10, 10); // 设置数据点的大小

        Image dotImage = dotGameObject.GetComponent<Image>();
        dotImage.sprite = dotSprite;
        dotImage.color = dotColor;

        return dotRectTransform;
    }

    void DrawLineBetweenDots(Vector2 dot1, Vector2 dot2)
    {
        // 创建UI Image作为线段
        GameObject lineGameObject = new GameObject("Line", typeof(Image));
        lineGameObject.transform.SetParent(graphContainer, false);

        RectTransform lineRectTransform = lineGameObject.GetComponent<RectTransform>();
        Vector2 direction = (dot2 - dot1).normalized;
        float distance = Vector2.Distance(dot1, dot2);
        lineRectTransform.sizeDelta = new Vector2(distance, 2); // 设置线段的长度和宽度
        lineRectTransform.anchoredPosition = dot1 + direction * distance * 0.5f;
        lineRectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        Image lineImage = lineGameObject.GetComponent<Image>();
        lineImage.color = dotColor;
    }
}