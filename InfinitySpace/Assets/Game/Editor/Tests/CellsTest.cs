using Assets.Game.Access;
using Assets.Game.Field.Cells;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Field.Generators.Iterators;

using NUnit.Framework;

using System.Linq;

using UnityEngine;

namespace Assets.Game.Editor.Tests
{
    public class CellsTest
    {
        /// <summary>
        /// Проверка получения координат ячейки по координатам позиции поля
        /// </summary>
        [Test]
        public void CellBeginPositionCheck()
        {
            Vector2Int[] positions = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(50, 50),
                new Vector2Int(99, 99),
                new Vector2Int(-50, -50),
                new Vector2Int(-99, -99),
                new Vector2Int(-1, -1),
                new Vector2Int(-99, 100),
            };

            Vector2Int[] cellPositions = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 0),
                new Vector2Int(0, 0),
                new Vector2Int(-100, -100),
                new Vector2Int(-100, -100),
                new Vector2Int(-100, -100),
                new Vector2Int(-100, 100),
            };

            Assert.AreEqual(positions.Length, cellPositions.Length);

            for (int i = 0; i < positions.Length; i++)
            {
                Assert.AreEqual(cellPositions[i], SettingsAccess.GetCellBeginPosition(positions[i]), string.Format("for {0}", i));
            }
        }

        /// <summary>
        /// Проверка <see cref="LinePositionIterator"/>
        /// </summary>
        [Test]
        public void LineEnumeratorCheck()
        {
            var rect = SettingsAccess.GetFieldRectPx(Vector2Int.zero);

            CheckLine(rect, 100, 100);
            CheckLine(rect, 100, 1000);
            CheckLine(rect, 1000, 100);
            CheckLine(rect, 1000, 1000);
        }

        /// <summary>
        /// Проверить <see cref="LinePositionIterator"/>
        /// </summary>
        /// <param name="rect">Прямоугольник для проверки</param>
        /// <param name="texWidth">Ширина текстуры</param>
        /// <param name="texHeight">Высота текстуры</param>
        private void CheckLine(RectInt rect, int texWidth, int texHeight)
        {
            int sizeX = rect.width / texWidth;
            int sizeY = rect.height / texHeight;

            int lenght = sizeX * sizeY;

            var lineEnum = new LinePositionIterator(new GenerateRectInfo(rect, texWidth, texHeight, -1, new PixelsSliceData()));

            Assert.AreEqual(lenght, lineEnum._count);

            int posX = rect.xMin;
            int posY = rect.yMin;

            var centerPos = Vector2Int.zero;

            while (lineEnum.MoveNext())
            {
                var pos = new Vector2Int(posX, posY);

                Assert.AreEqual(lineEnum.Current, pos);

                posX += texWidth;

                if (posX == rect.xMax)
                {
                    posY += texHeight;
                    posX = rect.xMin;
                }
            }
        }

        /// <summary>
        /// Проверка расчёта индекса по координатам поля
        /// </summary>
        [Test]
        public void CellsIndexCheck()
        {
            Vector2Int[] exists = new Vector2Int[]
            {
                new Vector2Int(6000, -6000)
            };

            Vector2Int[] check = new Vector2Int[]
            {
                new Vector2Int(-6000, -6000)
            };

            for (int i = 0; i < exists.Length; i++)
            {
                Assert.AreNotEqual(SettingsAccess.GetFieldIndex(exists[i]), SettingsAccess.GetFieldIndex(check[i]));
            }

            uint[] idList = new uint[SettingsAccess.FieldLength];

            for (int x = 0; x < SettingsAccess.FieldSize; x++)
            {
                for (int y = 0; y < SettingsAccess.FieldSize; y++)
                {
                    var index = SettingsAccess.GetFieldIndex(new Vector2Int(x, y) * SettingsAccess.CellPxSize);

                    Assert.AreEqual(idList[index], 0);

                    idList[index] = index;
                }
            }

            for (int x = SettingsAccess.FieldSize * -3; x < SettingsAccess.FieldSize * 4; x++)
            {
                for (int y = SettingsAccess.FieldSize * -3; y < SettingsAccess.FieldSize * 4; y++)
                {
                    var index = SettingsAccess.GetFieldIndex(new Vector2Int(x, y) * SettingsAccess.CellPxSize);

                    Assert.AreEqual(idList[index], index);
                }
            }
        }

        /// <summary>
        /// Проверка радиуса спирали
        /// </summary>
        [Test]
        public void CellRadiusCheck()
        {
            for (int radius = 1; radius < 10; radius++)
            {
                var size = SettingsAccess.RadiusToLength(radius);

                for (int index = 0; index < size; index++)
                {
                    var pos = SpiralPositionsIterator.Spiral(index) * radius;

                    Assert.GreaterOrEqual(radius, SettingsAccess.GetRadius(Vector2Int.zero, pos));
                }
            }
        }

        /// <summary>
        /// Проверка сортировки первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> результатов, ближайших про рейтингу к игроку
        /// </summary>
        [Test]
        public void CellItemsCheck()
        {
            CheckCellItemsSize(SettingsAccess.MaxAdvancedVisiblePlanet);

            CheckCellItemsSize(SettingsAccess.FieldLength);

            CheckCellItemsSize(Random.Range(100, 100000));
        }

        /// <summary>
        /// Проверка сортировки первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> результатов, ближайших про рейтингу к игроку
        /// </summary>
        /// <param name="size">Размер массива для проверки</param>
        private void CheckCellItemsSize(int size)
        {
            ushort playerRating = (ushort)Random.Range(1, 10002);

            var toAppendCells = new ushort[size];

            for (int i = 0; i < toAppendCells.Length; i++)
            {
                toAppendCells[i] = (ushort)Random.Range(0, 10000);
            }

            var sortedCells = new SortedCellItems();

            for (int i = 0; i < toAppendCells.Length; i++)
            {
                sortedCells.AppendCellItemRecommended(Vector2Int.zero, 0, toAppendCells[i], playerRating);
            }

            var toAppendCellsSorted = toAppendCells.Select(p=>(ushort)Mathf.Abs(p - playerRating)).OrderBy(p => p).Take(SettingsAccess.MaxAdvancedVisiblePlanet).ToArray();
            var toCheckCellsSorted = sortedCells.GetValues().Select(p => p.RatingDistance).OrderBy(p => p).ToArray();

            Assert.AreEqual(toCheckCellsSorted.Length, SettingsAccess.MaxAdvancedVisiblePlanet);

            for (int i = 0; i < SettingsAccess.MaxAdvancedVisiblePlanet; i++)
            {
                Assert.AreEqual(toAppendCellsSorted[i], toCheckCellsSorted[i]);
            }
        }
    }
}
