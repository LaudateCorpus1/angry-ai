﻿using UnityEngine;

namespace MLGridSensor
{
    /// <summary>
    /// Draws each <see cref="PixelGrid"/> layer into a GUI texture.
    /// </summary>
    [RequireComponent(typeof(IPixelGridProvider))]
    public class PixelGridVisualizer : MonoBehaviour
    {
        [SerializeField, Range(1, 16)]
        private int m_Magnify = 8;
        [SerializeField, Range(0, 4)]
        private int m_Offset = 0;

        private PixelGrid m_Grid;
        private Texture2D[] m_Textures;

        private void Initialize()
        {
            m_Textures = new Texture2D[m_Grid.Layers];
            for (int i = 0; i < m_Grid.Layers; i++)
            {
                m_Textures[i] = new Texture2D(m_Grid.Width, m_Grid.Width, TextureFormat.RGB24, false);
                m_Textures[i].filterMode = FilterMode.Point;
                m_Textures[i].wrapMode = TextureWrapMode.Clamp;
            }
        }

        private void OnGUI()
        {
            if (m_Grid == null)
            {
                m_Grid = GetComponent<IPixelGridProvider>().GetPixelGrid();
                Initialize();
            }

            int i = 0;
            // Flip vertically, spacing equals magnification factor.
            Rect rect = new Rect(
                m_Magnify + (m_Grid.Width + 1) * m_Magnify * m_Offset, 
                m_Magnify * (m_Grid.Height + 1),
                m_Grid.Width * m_Magnify, 
                m_Grid.Height * -m_Magnify);

            foreach (Color32[] colors in m_Grid.GetColors())
            {
                m_Textures[i].SetPixels32(colors);
                m_Textures[i].Apply();
                GUI.DrawTexture(rect, m_Textures[i]);
                rect.y += (m_Grid.Width + 1) * m_Magnify;
                i++;
            }
        }
    }
}