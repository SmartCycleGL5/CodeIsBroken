using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphDrawer : VisualElement
{
    
    VectorImage userGraph;
    private void Start()
    {
        LineGraphView();
    }

    public void LineGraphView()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        Painter2D painter2D = mgc.painter2D;
        
        painter2D.lineWidth = 10.0f;
        painter2D.strokeColor = Color.white;
        painter2D.lineJoin = LineJoin.Round;
        painter2D.lineCap = LineCap.Round;

        painter2D.BeginPath();
        painter2D.MoveTo(new Vector2(100, 100));
        painter2D.LineTo(new Vector2(120, 120));
        painter2D.LineTo(new Vector2(140, 100));
        painter2D.LineTo(new Vector2(160, 120));
        painter2D.LineTo(new Vector2(180, 100));
        painter2D.LineTo(new Vector2(200, 120));
        painter2D.LineTo(new Vector2(220, 100));
        painter2D.Stroke();

        painter2D.SaveToVectorImage(userGraph);
    }
}
