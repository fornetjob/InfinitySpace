using Assets.Game.Access;
using Assets.Game.Core;
using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.UI.Controls
{
    /// <summary>
    /// Зум поля
    /// </summary>
    public class ZoomControl : MonoBehaviour
    {
        #region Statics

        /// <summary>
        /// Возвращает видимую область поля
        /// </summary>
        /// <param name="playerPosition">Позиция игрока</param>
        /// <param name="realZoomValue">Зум</param>
        /// <returns>Видимый прямоугольник поля</returns>
        public static RectInt GetVisibleRect(Vector2Int playerPosition, int realZoomValue)
        {
            int size = realZoomValue;

            var sizeVector = Vector2Int.one * size;

            Vector2Int halfSize;

            if (IsCeilView(realZoomValue))
            {
                halfSize = Vector2Int.one * Mathf.CeilToInt(size / 2f);
            }
            else
            {
                halfSize = Vector2Int.one * Mathf.CeilToInt(size / 2f - 1);
            }

            return new RectInt(playerPosition - halfSize, sizeVector);
        }

        /// <summary>
        /// Это режим просмотра в ячейках?
        /// </summary>
        /// <param name="realZoomValue">Зум</param>
        /// <returns>Результат проверки</returns>
        private static bool IsCeilView(int realZoomValue)
        {
            return realZoomValue >= SettingsAccess.CellPxSize;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Шаг орфографического размера камеры 
        /// </summary>
        private const float OrthographicSizeStep = 0.5f;
        /// <summary>
        /// Максимальный орфографический размер камеры
        /// </summary>
        private const float MaxOrthographicSize = 5f;
        /// <summary>
        /// Минимальный зум
        /// </summary>
        private const int MinZoomIndex = 5;
        /// <summary>
        /// Особый режим отображения объектов
        /// </summary>
        private const int AdvancedZoomIndex = 10;
        /// <summary>
        /// Максимальный зум
        /// </summary>
        private const int MaxZoomIndex = 150;
        /// <summary>
        /// Максимальный фактический зум
        /// </summary>
        private static readonly int MaxRealZoom = Math.Min(10000, SettingsAccess.FieldLength);

        #endregion

        #region Mappings

        /// <summary>
        /// Маппинг сетку
        /// </summary>
        [Mapping(AbsolutePath = "FieldCamera/Grid")]
        [SerializeField]
        private Transform
            _gridTr;

        /// <summary>
        /// Кнопка увеличения зума
        /// </summary>
        [Mapping]
        [SerializeField]
        private Button
            _increaseButton;

        /// <summary>
        /// Кнопка уменьшения зума
        /// </summary>
        [Mapping]
        [SerializeField]
        private Button
            _decreaseButton;

        /// <summary>
        /// Текст зума
        /// </summary>
        [Mapping]
        [SerializeField]
        private TextMeshProUGUI
            _zoomValue;

        /// <summary>
        /// Слайдер зума
        /// </summary>
        [Mapping]
        [SerializeField]
        private Slider
            _zoomSlider;

        /// <summary>
        /// Маппинг на камеру для зума
        /// </summary>
        [Mapping(AbsolutePath = "FieldCamera")]
        [SerializeField]
        private Camera
            _zoomCamera;

        #endregion

        #region Fields

        /// <summary>
        /// Текущий зум
        /// </summary>
        private int
            _currentZoom;

        #endregion

        #region Events

        /// <summary>
        /// Зум изменён
        /// </summary>
        public event Action ZoomChanged;

        #endregion

        #region Public methods

        /// <summary>
        /// Приступить
        /// </summary>
        public void Begin()
        {
            gameObject.SetActive(true);

            float aspect = (float)Screen.width / (float)Screen.height;
            float scalewidth = 1f / aspect;

            _zoomCamera.rect = new Rect(0, 0, scalewidth, 1f);

            _currentZoom = MinZoomIndex;

            _increaseButton.onClick.RemoveAllListeners();
            _increaseButton.onClick.AddListener(OnIncreaseZoom);

            _decreaseButton.onClick.RemoveAllListeners();
            _decreaseButton.onClick.AddListener(OnDecreaseZoom);

            _zoomSlider.onValueChanged.RemoveAllListeners();
            _zoomSlider.onValueChanged.AddListener(OnValueChanged);

            OnZoomChanged();
        }

        /// <summary>
        /// Возвращает точность зума
        /// </summary>
        /// <returns></returns>
        public float GetZoomPrecision()
        {
            int realZoomValue = GetRealZoomValue();

            return Math.Min(1, 1 / (realZoomValue / (float)(AdvancedZoomIndex - 1)));
        }

        /// <summary>
        /// Возвращает видимую область для указанных координат игрока
        /// </summary>
        /// <param name="playerPosition">Координаты позиции игрока</param>
        /// <returns>Прямоугольник видимой области</returns>
        public RectInt GetVisibleRect(Vector2Int playerPosition)
        {
            int realZoomValue = GetRealZoomValue();

            return GetVisibleRect(playerPosition, realZoomValue);
        }

        /// <summary>
        /// Это особый режим отображения объектов
        /// </summary>
        /// <returns></returns>
        public bool IsAdvancedView()
        {
            return GetRealZoomValue() >= AdvancedZoomIndex;
        }

        /// <summary>
        /// Это режим отображения, в котором происходит центрирование камеры по ячейкам
        /// </summary>
        /// <returns></returns>
        public bool IsCellsView()
        {
            return _currentZoom > SettingsAccess.CellPxSize;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Нажата кнопка увеличения зума
        /// </summary>
        private void OnIncreaseZoom()
        {
            AppendZoom(1);
        }

        /// <summary>
        /// Нажата кнопка уменьшения зума
        /// </summary>
        private void OnDecreaseZoom()
        {
            AppendZoom(-1);
        }

        /// <summary>
        /// Событие изменения значения слайдера
        /// </summary>
        /// <param name="floatValue">Текущее значение слайдера</param>
        private void OnValueChanged(float floatValue)
        {
            int value = (int)floatValue;

            value = Mathf.Min(MaxZoomIndex, value);
            value = Mathf.Max(MinZoomIndex, value);

            if (value != _currentZoom)
            {
                _currentZoom = value;

                OnZoomChanged();
            }
        }

        /// <summary>
        /// Зум изменён
        /// </summary>
        private void OnZoomChanged()
        {
            _zoomCamera.orthographicSize = Mathf.Min(_currentZoom * OrthographicSizeStep, MaxOrthographicSize);

            var zoomValue = GetRealZoomValue();

            _zoomValue.text = zoomValue.ToString();

            var isGridActive = IsAdvancedView() == false;

            if (_gridTr.gameObject.activeSelf != isGridActive)
            {
                _gridTr.gameObject.SetActive(isGridActive);
            }

            if (ZoomChanged != null)
            {
                ZoomChanged();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Изменение зума
        /// </summary>
        /// <param name="append">Значение</param>
        private void AppendZoom(int append)
        {
            _currentZoom += append;

            if (_currentZoom < MinZoomIndex)
            {
                _currentZoom = MinZoomIndex;

                return;
            }

            if (_currentZoom > MaxZoomIndex)
            {
                _currentZoom = MaxZoomIndex;

                return;
            }

            OnZoomChanged();

            _zoomSlider.value = _currentZoom;
        }

        /// <summary>
        /// Получить фактическое значение зума
        /// </summary>
        /// <returns></returns>
        private int GetRealZoomValue()
        {
            if (IsCellsView() == false)
            {
                return _currentZoom;
            }

            return Math.Min(MaxRealZoom, (_currentZoom - SettingsAccess.CellPxSize) * SettingsAccess.CellPxSize * 2 + SettingsAccess.CellPxSize);
        }

        #endregion
    }
}
