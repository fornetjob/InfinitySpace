using Assets.Game.Access;
using Assets.Game.Access.Enums;
using Assets.Game.Core;
using Assets.Game.Core.PoolingSystem;
using Assets.Game.Field.Base;
using Assets.Game.Field.Cells;
using Assets.Game.Field.Generators;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Field.Items;
using Assets.Game.Inputs;
using Assets.Game.Inputs.Base;
using Assets.Game.Tools;
using Assets.Game.UI.Controls;

using UnityEngine;

namespace Assets.Game.Field
{
    /// <summary>
    /// Поведение поля
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class FieldBehaviour:MonoBehaviour
    {
        #region Mappings

        /// <summary>
        /// Маппинг на Transform поля
        /// </summary>
        [Mapping(".")]
        [SerializeField]
        private Transform
            _tr;

        #endregion

        #region Fields

        /// <summary>
        /// Пулинг планет
        /// </summary>
        private PrefabsPoolingManager<Planet>
            _planets = new PrefabsPoolingManager<Planet>(PrefabType.Planet);

        /// <summary>
        /// Коллекция ячеек поля
        /// </summary>
        private CellCollection
            _cells;

        /// <summary>
        /// Зум поля
        /// </summary>
        private ZoomControl
            _zoom;

        /// <summary>
        /// Генерация ячеек поля
        /// </summary>
        private IAsyncCollectionProcess<GenerateRectInfo>
            _generator;

        /// <summary>
        /// Поиск первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> планет, ближайших по рейтингу к рейтингу игрока
        /// </summary>
        private IAsyncCollectionProcess<SortedCellItem[]>
            _sortedCellsVisitor;

        /// <summary>
        /// Игрок
        /// </summary>
        private Player
            _player;

        /// <summary>
        /// Рейтинг игрока
        /// </summary>
        private ushort
            _playerRating;
        
        /// <summary>
        /// Предыдущий прямоугольник камеры
        /// </summary>
        private RectInt?
            _prevRect;

        /// <summary>
        /// Поле инициализировано
        /// </summary>
        private bool
            _isInited = false;

        /// <summary>
        /// Координаты текущая ячейки игрока на поле
        /// </summary>
        private Vector2Int
            _currentCellPosition;

        /// <summary>
        /// Координаты текущей позиция игрока на поле
        /// </summary>
        private Vector2Int 
            _currentCellItemPosition;

        private IPlayerInput
            _input;

        #endregion

        #region Game

        void Update()
        {
            if (_isInited == false)
            {
                return;
            }

            // Управление игроком
            var direction = _input.GetDirection();

            if (direction != Vector2Int.zero)
            {
                _currentCellItemPosition = _currentCellItemPosition + direction;

                OnPlayerPositionChanged();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Инициализация поля
        /// </summary>
        /// <param name="zoom">Зум поля</param>
        /// <param name="progress">Прогресс инициализации</param>
        public void Init(ZoomControl zoom, ProgressBar progress, IPlayerInput input)
        {
            // Подпишемся на изменения зума
            _zoom = zoom;

            _input = input;

            // Инициализируем случайные значения положения игрока на поле и его рейтинг
            _currentCellPosition = SettingsAccess.GetRandomFieldPosition();
            _currentCellItemPosition = SettingsAccess.GetRandomCellPosition(_currentCellPosition);
            _playerRating = (ushort)Random.Range(1, SettingsAccess.MaxRating);

            // Инициализируем коллекцию ячеек
            _cells = new CellCollection(_playerRating);

            // Инициализируем механизм поиска первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> планет, ближайших по рейтингу к рейтингу игрока
            _sortedCellsVisitor = gameObject.AddComponent<SortedCellsVisitor>().Init(_cells, _playerRating);
            _sortedCellsVisitor.OnProcessEnd += OnSearchTopRatingsEnd;

            string generationString;

            if (SystemInfo.supportsComputeShaders)
            {
                // Если поддерживаются Computed Shader
                _generator = gameObject.AddComponent<ComputedShaderNoiseGenerator>();

                generationString = "GENERATE COMPUTED SHADER";
            }
            else
            {
                // Для всех остальных платформ.
                // По идее, должен поддерживаться большинством платформ (если это не так, нужно добавить для недостающих платформ свои реализации <typeparamref name="NoiseGeneratorBase")
                _generator = gameObject.AddComponent<CustomRenderTextureNoiseGenerator>();

                generationString = "GENERATE RENDER TEXTURE";
            }

            _generator.OnProcessEnd += OnGenerationEnd;

            // Добавим первое задание на генерацию всего поля
            _generator.AppendRect(SettingsAccess.GetFieldRectPx(_currentCellPosition));

            // Отобразим прогресс
            progress.Begin(() => _generator.GetProgress(), generationString, OnEndInit);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Событие окончания генерации куска поля
        /// </summary>
        /// <param name="info"></param>
        private void OnGenerationEnd(GenerateRectInfo info)
        {
            _cells.FillRect(_currentCellPosition, info);
        }

        /// <summary>
        /// Событие окончания инициализации поля
        /// </summary>
        private void OnEndInit()
        {
            // Добавим игрока на поле
            _player = ResourcesAccess.Instance.CreatePrefab<Player>(PrefabType.Player);
            _player.Show(_playerRating);

            // Инициализируем зум
            _zoom.ZoomChanged += OnZoomChanged;
            _zoom.Begin();

            // Включим управление
            _input.SetState(true);

            _isInited = true;
        }

        /// <summary>
        /// Событие окончания поиска первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> планет, ближайших по рейтингу к рейтингу игрока
        /// </summary>
        /// <param name="values">Результаты поиска</param>
        private void OnSearchTopRatingsEnd(SortedCellItem[] values)
        {
            FillAdvancedVisiblePlanets(values);
        }

        /// <summary>
        /// Изменился зум
        /// </summary>
        private void OnZoomChanged()
        {
            OnPlayerPositionChanged();
        }

        /// <summary>
        /// Изменилась текущая позиция игрока на поле
        /// </summary>
        private void OnPlayerPositionChanged()
        {
            var newCellPosition = SettingsAccess.GetCellBeginPosition(_currentCellItemPosition);

            // Если изменилась текущая ячейка игрока на поле, добавим разницу между прямоугольниками старых и новых координат игрока на генерацию
            if (newCellPosition != _currentCellPosition)
            {
                var prevRect = SettingsAccess.GetFieldRectPx(_currentCellPosition);
                var prevVisibleRect = SettingsAccess.GetFullGenerationRect(_currentCellPosition);

                var newRect = SettingsAccess.GetFieldRectPx(newCellPosition);
                var newVisibleRect = SettingsAccess.GetFullGenerationRect(newCellPosition);

                // Разница между старым прямоугольником всего поля и текущим
                var rectsToFill = RectIntTool.Subtract(newRect, prevRect);
                // Разница между старым прямоугольником полностью генерируемого мира вокруг игрока и текущим
                var visibleRectsFill = RectIntTool.Subtract(newVisibleRect, prevVisibleRect);

                visibleRectsFill.AddRange(rectsToFill);

                if (visibleRectsFill.Count > 0)
                {
                    // Добавить задание на генерацию - сперва прямоугольники полностью генерируемых ячеек вокруг игрока, потом прямоугольники поля
                    _generator.AppendRects(visibleRectsFill, true);
                }

                _currentCellPosition = newCellPosition;
            }

            if (_zoom.IsAdvancedView() == false)
            {
                RefreshNormalVisibleArea();

                SetPosition();
            }
            else
            {
                if (_zoom.IsCellsView())
                {
                    _sortedCellsVisitor.AppendRect(_zoom.GetVisibleRect(_currentCellPosition));
                }
                else
                {
                    _sortedCellsVisitor.AppendRect(_zoom.GetVisibleRect(_currentCellItemPosition));
                }
            }
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Установить текущую позицию поля и игрока
        /// </summary>
        private void SetPosition()
        {
            var playerPos = SettingsAccess.ConvertToWorldPosition(_currentCellItemPosition, _zoom.GetZoomPrecision());

            if (_zoom.IsCellsView())
            {
                _tr.position = SettingsAccess.ConvertToWorldPosition(_currentCellPosition + Vector2Int.one * SettingsAccess.HalfCellPxSize , _zoom.GetZoomPrecision());
            }
            else
            {
                _tr.position = playerPos;
            }

            _player.SetPosition(playerPos);
        }

        /// <summary>
        /// Обновить видимую область при обычном режиме просмотра планет
        /// </summary>
        private void RefreshNormalVisibleArea()
        {
            var rect = _zoom.GetVisibleRect(_currentCellItemPosition);

            if (_prevRect == null)
            {
                _planets.DestroyAll();

                FillPlanets(rect);
            }
            else
            {
                // Вычтем текущего прямоугольник из прошлого, получив таким образом область для очистки
                var rectsToDestroy = RectIntTool.Subtract(_prevRect.Value, rect);

                for (int i = 0; i < rectsToDestroy.Count; i++)
                {
                    ClearPlanets(rectsToDestroy[i]);
                }

                // Вычтем прошлый прямоугольник из текущего, получив таким образом область для заполнения планетами
                var rectsToFill = RectIntTool.Subtract(rect, _prevRect.Value);

                for (int i = 0; i < rectsToFill.Count; i++)
                {
                    FillPlanets(rectsToFill[i]);
                }
            }

            _prevRect = rect;
        }

        /// <summary>
        /// Очистить прямоугольник от планет
        /// </summary>
        /// <param name="rect">Прямоугольник который требуется очистить от планет</param>
        private void ClearPlanets(RectInt rect)
        {
            var iterator = rect.allPositionsWithin;

            while (iterator.MoveNext())
            {
                var posId = SettingsAccess.GetId(iterator.Current);

                var planet = _planets.GetByIdOrNull(posId);

                if (planet != null)
                {
                    planet.Destroy();
                }
            }
        }

        /// <summary>
        /// Отобразить планеты, входящие в прямоугольник
        /// </summary>
        /// <param name="rect">Прямоугольник в который входят планеты, которые требуется создать</param>
        private void FillPlanets(RectInt rect)
        {
            var iterator = rect.allPositionsWithin;

            CellInfo cell = null;

            while (iterator.MoveNext())
            {
                var worldPos = iterator.Current;
                var cellPos = SettingsAccess.GetCellBeginPosition(worldPos);

                if (cell == null
                    || cell.Pos != cellPos)
                {
                    cell = _cells.GetCell(cellPos);
                }

                var cellItemPos = cell.GetCellItemPosition(worldPos);

                var rating = cell.GetRating(cellItemPos);

                if (rating > 0)
                {
                    var posId = SettingsAccess.GetId(worldPos);

                    var planet = _planets.Create(posId);

                    planet.Show(worldPos, SettingsAccess.ConvertToWorldPosition(worldPos), rating);
                }
            }
        }

        /// <summary>
        /// Создать планеты из списка
        /// </summary>
        /// <param name="cellItems">Список планет для создания</param>
        private void FillAdvancedVisiblePlanets(SortedCellItem[] cellItems)
        {
            _planets.DestroyAll();

            var precision = _zoom.GetZoomPrecision();

            for (int i = 0; i < cellItems.Length; i++)
            {
                var cellItem = cellItems[i];

                var cell = _cells.GetCell(cellItem.CellPos);

                var worldPos = cell.GetWordPosition(cellItem.Index);

                var posId = SettingsAccess.GetId(worldPos);

                var planet = _planets.Create(posId);

                planet.Show(worldPos, SettingsAccess.ConvertToWorldPosition(worldPos, precision), cellItem.Rating);
            }

            SetPosition();

            _prevRect = null;
        }

        #endregion
    }
}