using Assets.Game.Editor.Tools;
using Assets.Game.Field.Cells;
using Assets.Game.Tools;
using Assets.Game.UI.Controls;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Assets.Game.Field.Editor
{
    [CustomEditor(typeof(FieldBehaviour))]
    public class FieldBehaviourEditor: UnityEditor.Editor
    {
        private CheckDelay
            _check = new CheckDelay(1000);

        public FieldBehaviour Target
        {
            get
            {
                return (FieldBehaviour)target;
            }
        }

        void OnSceneGUI()
        {
            if (_check.Check())
            {
                DrawHelpers();
            }
        }

        private void DrawHelpers()
        {
            var cells = EditorTool.GetField<CellCollection>(Target, "_cells");

            if (cells == null)
            {
                return;
            }

            var currentCellPos = EditorTool.GetField<Vector2Int>(Target, "_currentCellPosition");

            var zoom = EditorTool.GetField<ZoomControl>(Target, "_zoom");

            var precision = zoom.GetZoomPrecision();

            for (int i = 0; i < cells._cells.Length; i++)
            {
                var cell = cells._cells[i];

                if (cell == null)
                {
                    continue;
                }

                var rect = cell.GetRect(precision);

                bool isVisible = cells.IsFullGeneratedCell(currentCellPos, cell.Pos);

                DrawRect(rect, isVisible ? Color.blue : Color.red, 1f, precision);
            }
        }

        private void DrawRect(Rect rect, Color color, float duration, float precision)
        {
            float deflateValue = precision;

            float xMin = rect.xMin + deflateValue;
            float xMax = rect.xMax - deflateValue;

            float yMin = rect.yMin + deflateValue;
            float yMax = rect.yMax - deflateValue;

            Debug.DrawLine(new Vector2(xMin, yMin), new Vector2(xMax, yMin), color, duration);
            Debug.DrawLine(new Vector2(xMax, yMin), new Vector2(xMax, yMax), color, duration);
            Debug.DrawLine(new Vector2(xMax, yMax), new Vector2(xMin, yMax), color, duration);
            Debug.DrawLine(new Vector2(xMin, yMax), new Vector2(xMin, yMin), color, duration);
        }
    }
}