// LineGraph_UI_Toolkit.cs
// A reusable VisualElement that draws a simple line graph using Unity UI Toolkit's Vector API
// Compatible with Unity 2022.1+ (uses generateVisualContent and MeshGenerationContext.painter2D)

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LineGraphElement : VisualElement
{
    
    // Public styling options
    public Color gridColor = new Color(1f,1f,1f,1f);
    public Color axisColor = new Color(1f,1f,1f,0.25f);
    public float lineWidth = 5;
    public float pointRadius = 3f;

    List<int> m_Data = new List<int>();
    List<int> m_DataBaseline = new List<int>();
    float m_Min = 0f, m_Max = 1f;

    public LineGraphElement()
    {
        generateVisualContent += OnGenerateVisualContent;
        // default style so it gets a size if none provided
        style.width = Length.Percent(100);
        style.height = 200;
    }

    /// <summary>
    /// Set the data points for the graph. Call this to update and repaint.
    /// </summary>
    public void SetData(List<int> data, List<int> dataBaseline)
    {
        m_Data = data ?? new List<int>();
        m_DataBaseline = dataBaseline ?? new List<int>();
        RecalculateMinMax();
        MarkDirtyRepaint();
    }

    void RecalculateMinMax()
    {
        if (m_Data == null || m_Data.Count == 0)
        {
            m_Min = 0f; m_Max = 1f; return;
        }
        m_Min = m_Max = m_Data[0];
        for (int i = 1; i < m_Data.Count; i++)
        {
            if (m_Data[i] < m_Min) m_Min = m_Data[i];
            if (m_Data[i] > m_Max) m_Max = m_Data[i];
        }
        if (Mathf.Approximately(m_Min, m_Max)) { m_Min -= 0.5f; m_Max += 0.5f; }
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var painter = mgc.painter2D;
        var r = contentRect; // pixel rect of this element

        // Draw background grid (optional)
        DrawGrid(painter, r);

        // Draw axes
        painter.strokeColor = axisColor;
        painter.lineWidth = 10f;
        painter.BeginPath();
        // Y axis (left)
        painter.MoveTo(new Vector2(r.xMin + 10f, r.yMin + 4f));
        painter.LineTo(new Vector2(r.xMin + 10f, r.yMax - 4f));
        // X axis (bottom)
        painter.MoveTo(new Vector2(r.xMin + 6f, r.yMax - 10f));
        painter.LineTo(new Vector2(r.xMax - 4f, r.yMax - 10f));
        painter.Stroke();

        if (m_Data == null || m_Data.Count == 0)
            return;

        // Map data to points inside a padded rect
        float padLeft = 14f, padRight = 6f, padTop = 6f, padBottom = 14f;
        float w = Mathf.Max(1f, r.width - padLeft - padRight);
        float h = Mathf.Max(1f, r.height - padTop - padBottom);

        Vector2 ToPoint(int index, float value)
        {
            float t = (m_Data.Count == 1) ? 0.5f : (float)index / (m_Data.Count - 1);
            float x = r.xMin + padLeft + t * w;
            float normalized = (value - m_Min) / (m_Max - m_Min);
            // inverse Y because UI origin is top-left
            float y = r.yMax - padBottom - normalized * h;
            return new Vector2(x, y);
        }

        // Draw polyline
        painter.strokeColor = Color.green;
        painter.lineWidth = lineWidth;

        // Plot users performace
        painter.BeginPath();
        painter.MoveTo(ToPoint(0, m_Data[0]));
        for (int i = 1; i < m_Data.Count; i++)
            painter.LineTo(ToPoint(i, m_Data[i]));
        painter.Stroke();
        
        painter.strokeColor = Color.blue;
        // Plot for baseline
        painter.BeginPath();
        painter.MoveTo(ToPoint(0, m_DataBaseline[0]));
        for (int i = 1; i < m_DataBaseline.Count; i++)
            painter.LineTo(ToPoint(i, m_DataBaseline[i]));
        painter.Stroke();

        // Draw points
        painter.fillColor = Color.red;
        for (int i = 0; i < m_Data.Count; i++)
        {
            var p = ToPoint(i, m_Data[i]);
            painter.BeginPath();
            painter.Arc(p, pointRadius, 10, Mathf.PI * 2f);
            painter.ClosePath();
            painter.Fill();
        }
        // Draw point baseline
        painter.fillColor = Color.blue;
        for (int i = 0; i < m_DataBaseline.Count; i++)
        {
            var p = ToPoint(i, m_DataBaseline[i]);
            painter.BeginPath();
            painter.Arc(p, pointRadius, 10, Mathf.PI * 2f);
            painter.ClosePath();
            painter.Fill();
        }
    }

    void DrawGrid(Painter2D painter, Rect r)
    {
        painter.strokeColor = gridColor;
        painter.lineWidth = 2;

        int rows = 4;
        int cols = 6;
        for (int i = 0; i <= rows; i++)
        {
            float y = Mathf.Lerp(r.yMin + 6f, r.yMax - 10f, (float)i / rows);
            painter.BeginPath();
            painter.MoveTo(new Vector2(r.xMin + 6f, y));
            painter.LineTo(new Vector2(r.xMax - 4f, y));
            painter.Stroke();
            
        }
        for (int j = 0; j <= cols; j++)
        {
            float x = Mathf.Lerp(r.xMin + 14f, r.xMax - 6f, (float)j / cols);
            painter.BeginPath();
            painter.MoveTo(new Vector2(x, r.yMin + 6f));
            painter.LineTo(new Vector2(x, r.yMax - 10f));
            painter.Stroke();
        }
    }
}

// Helper MonoBehaviour to demonstrate usage at runtime.
public class GraphDrawer : MonoBehaviour
{
    [SerializeField] List<string> graphs;
    public static GraphDrawer instance;
    public UIDocument uiDocument;

    private void Start()
    {
        instance = this;
    }

    public void DrawCharts(params List<int>[] args)
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();
        
        uiDocument.rootVisualElement.Q<VisualElement>("GraphHolder").style.visibility = Visibility.Visible;
        
        for (int i = 0; i < graphs.Count; i++)
        {
            var root = uiDocument.rootVisualElement;
            var container = root.Q<VisualElement>(graphs[i]);

            var graph = new LineGraphElement();
            graph.style.width = Length.Percent(100);
            graph.style.height = 220;
            graph.style.marginTop = 10;

            // Example data
            var data = args[i];
            var dataBaseline = new List<int>() { 30,60,120};
            graph.SetData(data, dataBaseline);

            container.Add(graph);
        }
        
    }
}
