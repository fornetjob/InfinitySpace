using Assets.Game.Access;

using UnityEngine;

namespace Assets.Game.Editor.Tests.DataContracts
{
    /// <summary>
    /// Использование шейдера для сортировки ячеек. Разработка не вошла в билд.
    /// </summary>
    public class SortCells
    {
        #region Contants

        /// <summary>
        /// Количество потоков для ComputeShader
        /// </summary>
        public const int ShaderThreadCount = 1;

        /// <summary>
        /// Максимальный размер сортировки
        /// </summary>
        private const int MaxSortLength = 20;

        #endregion

        #region Fields

        /// <summary>
        /// Массив позиций
        /// </summary>
        private Vector3Int[]
            _posData = new Vector3Int[MaxSortLength];

        /// <summary>
        /// Массив значений
        /// </summary>
        private int[]
            _valuesData = new int[2];

        /// <summary>
        /// Массив дистанций
        /// </summary>
        private int[]
            _distanceData = new int[MaxSortLength];

        /// <summary>
        /// Буфер значений
        /// </summary>
        private ComputeBuffer
            _valuesBuffer = new ComputeBuffer(MaxSortLength, 4);

        /// <summary>
        /// Буфер дистанций
        /// </summary>
        private ComputeBuffer
            _distanceBuffer = new ComputeBuffer(MaxSortLength, 4);

        /// <summary>
        /// Буфер позиций
        /// </summary>
        private ComputeBuffer
            _posBuffer = new ComputeBuffer(MaxSortLength, 4 * 3);

        /// <summary>
        /// Результаты сортировки
        /// </summary>
        private SortedCell[]
            _results = new SortedCell[MaxSortLength];

        #endregion

        #region Public methods

        /// <summary>
        /// Сортировать
        /// </summary>
        /// <param name="size">Размер поля для сортировки</param>
        /// <returns>Результаты сортировки</returns>
        public SortedCell[] Sort(int size)
        {
            Debug.Log(SystemInfo.supportsComputeShaders);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            var pos = Vector2Int.zero;

            var shader = ResourcesAccess.Instance.SortShader;

            int kernelIndex = shader.FindKernel("Sort");

            shader.SetInt("PosX", pos.x);
            shader.SetInt("PosY", pos.x);
            shader.SetInt("PlayerRating", 1);
            shader.SetInt("SortLength", MaxSortLength);

            var length = size * size;

            ClearValuesData();
            ClearDistanceData();

            shader.SetBuffer(kernelIndex, "Values", _valuesBuffer);
            shader.SetBuffer(kernelIndex, "DistanceBuffer", _distanceBuffer);
            shader.SetBuffer(kernelIndex, "PosBuffer", _posBuffer);

            _valuesBuffer.SetData(_valuesData);
            _distanceBuffer.SetData(_distanceData);

            shader.Dispatch(kernelIndex, length / ShaderThreadCount, 1, 1);

            timer.Stop();

            Debug.Log(timer.ElapsedMilliseconds);
            timer.Reset();

            timer.Start();

            _posBuffer.GetData(_posData);

            timer.Stop();

            Debug.Log(timer.ElapsedMilliseconds);

            for (int i = 0; i < _results.Length; i++)
            {
                var data = _posData[i];

                _results[i] = new SortedCell
                {
                    X = data.x,
                    Y = data.y,
                    Rating = (ushort)data.z
                };
            }

            return _results;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Очистить значения
        /// </summary>
        private void ClearValuesData()
        {
            _valuesData[0] = SettingsAccess.MaxRating;
            _valuesData[1] = 0;
        }

        /// <summary>
        /// Очистить список дистанций
        /// </summary>
        private void ClearDistanceData()
        {
            for (int i = 0; i < _distanceData.Length; i++)
            {
                _distanceData[i] = SettingsAccess.MaxRating;
            }
        }

        #endregion
    }
}
