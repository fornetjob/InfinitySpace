using Assets.Game.Editor.Tools;
using Assets.Game.Field.Cells;
using Assets.Game.Tools;
using Assets.Game.UI.Controls;

using UnityEditor;
using UnityEngine;

namespace Assets.Game.Field.Editor
{
    /// <summary>
    /// Редактор поля
    /// </summary>
    [CustomEditor(typeof(FieldBehaviour))]
    public class FieldBehaviourEditor: UnityEditor.Editor
    {
        #region Fields

        /// <summary>
        /// Задержка между перерисовкой
        /// </summary>
        private CheckDelay
            _check = new CheckDelay(1000);

        #endregion

        #region Properties

        /// <summary>
        /// Поле
        /// </summary>
        public FieldBehaviour Target
        {
            get
            {
                return (FieldBehaviour)target;
            }
        }

        #endregion

        #region Game

        void OnSceneGUI()
        {
            if (_check.Check())
            {
                DrawHelpers();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Отладочная информация поля
        /// </summary>
        private void DrawHelpers()
        {
            var cells = ReflectionTool.GetField<CellCollection>(Target, "_cells");

            if (cells == null)
            {
                return;
            }

            var currentCellPos = ReflectionTool.GetField<Vector2Int>(Target, "_currentCellPosition");

            var zoom = ReflectionTool.GetField<ZoomControl>(Target, "_zoom");

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

        /// <summary>
        /// Нарисовать прямоугольник
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="color">Цвет</param>
        /// <param name="duration">Время</param>
        /// <param name="precision">Точность</param>
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

        #endregion
    }
}