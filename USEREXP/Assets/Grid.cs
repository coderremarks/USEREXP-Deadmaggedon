using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    private int _width;
    private int _height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int p_width, int p_height, float p_cellSize, Vector3 p_originPosition)
    {
        this._width = p_width;
        this._height = p_height;
        this.cellSize = p_cellSize;
        this.originPosition = p_originPosition;

        gridArray = new int[p_width, p_height];
        debugTextArray = new TextMesh[p_width, p_height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) /*+ new Vector3(p_cellSize, p_cellSize) * .5f*/, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                Debug.DrawLine(GetWorldPosition(x , y), GetWorldPosition(x, y + 1), Color.green, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, p_height), GetWorldPosition(p_width, p_height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(p_width, 0), GetWorldPosition(p_width, p_height), Color.white, 100f);

        //SEMI MODDED
        //float borderOffset = -p_cellSize / (p_cellSize * 2);
        //for (int x = 0; x < gridArray.GetLength(0); x++)
        //{
        //    for (int y = 0; y < gridArray.GetLength(1); y++)
        //    {
        //        debugTextArray[x, y] = CreateWorldText(null, gridArray[x, y].ToString(), GetWorldPosition(x, y) /*+ new Vector3(p_cellSize, p_cellSize) * .5f*/, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
        //        Debug.DrawLine(GetWorldPosition(x + borderOffset, y + borderOffset), GetWorldPosition(x + borderOffset, y + borderOffset + 1), Color.green, 100f);
        //        Debug.DrawLine(GetWorldPosition(x + borderOffset, y + borderOffset), GetWorldPosition(x + borderOffset + 1, y + borderOffset), Color.green, 100f);
        //    }
        //}
        //Debug.DrawLine(GetWorldPosition(0 + borderOffset, p_height + borderOffset), GetWorldPosition(p_width + borderOffset, p_height + borderOffset), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(p_width + borderOffset, 0 + borderOffset), GetWorldPosition(p_width + borderOffset, p_height + borderOffset), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int p_x, int p_y)
    {
        return new Vector3(p_x, p_y) * cellSize + originPosition;
    }

    public void SetValue(int p_x, int p_y, int p_value)
    {
        if (p_x >= 0 && p_y >= 0 && p_x < _width && p_y < _height)
        {
            gridArray[p_x, p_y] = p_value;
            debugTextArray[p_x, p_y].text = gridArray[p_x, p_y].ToString();
        }
    }

    public void SetValue(Vector3 p_worldPosition, int p_value)
    {
        int x, y;
        GetXY(p_worldPosition, out x, out y);
        SetValue(x, y, p_value);
    }

    public int GetValue(int p_x, int p_y)
    {
        if (p_x >= 0 && p_y >= 0 && p_x < _width && p_y < _height)
        {
            return gridArray[p_x, p_y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 p_worldPosition)
    {
        int x, y;
        GetXY(p_worldPosition, out x, out y);
        return GetValue(x, y);
    }
    public void GetXY(Vector3 p_worldPosition, out int p_x, out int p_y)
    {
        p_x = Mathf.FloorToInt((p_worldPosition - originPosition).x / cellSize);
        p_y = Mathf.FloorToInt((p_worldPosition - originPosition).y / cellSize);
    }

    public TextMesh CreateWorldText(Transform p_parent, string p_text, Vector3 p_localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(p_parent, false);
        transform.localPosition = p_localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = p_text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }
}
