using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{

    private Material lineMaterial;

    public bool gridLines = true;

    public int gridSizeX;
    public int gridSizeY;

    public float startX;
    public float startY;
    public float startZ;

    public float step;

    public Color gridColor = new Color(0f, 0.5f, 0f, 1f);


    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);

            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // Turning on Alpha Blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turning off Depth Writing
            lineMaterial.SetInt("_ZWrite", 0);

            // Turning off Backface Culling
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        }
    }

    private void OnDisable()
    {
        DestroyImmediate(lineMaterial);
    }

    private void OnPostRender()
    {
        CreateLineMaterial();

        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        if (gridLines)
        {
            GL.Color(gridColor);

            for (float y = 0; y <= gridSizeY; y += step)
            {
                GL.Vertex3(startX, startY + y, startZ);
                GL.Vertex3(startX + gridSizeX, startY + y, startZ);
            }

            for (float x = 0; x <= gridSizeX; x += step)
            {
                GL.Vertex3(startX + x, startY, startZ);
                GL.Vertex3(startX + x, startY + gridSizeY, startZ);
            }
        }

        GL.End();
    }

}
