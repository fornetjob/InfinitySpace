using Assets.Game.Tools;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Game.Editor.Tests
{
    public class RectangleTest
    {
        /// <summary>
        /// Проверка разницы между прямоугольников
        /// </summary>
        [Test]
        public void SubtractRectangleCheck()
        {
            RectInt[] rects = new RectInt[]
            {
                new RectInt(0, 0, 3, 3),
                new RectInt(1, 1, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 1, 3, 3),
                new RectInt(0, 1, 3, 3),
                new RectInt(1, 0, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(-1, -1, 3, 3),
                new RectInt(-1, -1, 5, 5),
                new RectInt(0, 0, 3, 3)
            };

            RectInt[] subtracts = new RectInt[]
            {
                new RectInt(1, 1, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 1, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(1, 0, 3, 3),
                new RectInt(0, 1, 3, 3),
                new RectInt(0, -1, 3, 3),
                new RectInt(-1, -1, 3, 3),
                new RectInt(3, 3, 3, 3),
                new RectInt(-3, -3, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(0, 0, 3, 3),
                new RectInt(-1, -1, 5, 5)
            };

            RectInt[][] results = new RectInt[][]
            {
                new RectInt[]
                {
                    new RectInt(0, 0, 3, 1),
                    new RectInt(0, 1, 1, 2 )
                },
                new RectInt[]
                {
                    new RectInt(3, 1, 1, 3),
                    new RectInt(1, 3, 2, 1 )
                },
                new RectInt[]
                {
                    new RectInt(0, 0, 3, 1),
                },
                new RectInt[]
                {
                    new RectInt(0, 3, 3, 1),
                },
                new RectInt[]
                {
                    new RectInt(0, 1, 1, 3),
                    new RectInt(1, 3, 2, 1 )
                },
                new RectInt[]
                {
                    new RectInt(1, 0, 3, 1),
                    new RectInt(3, 1, 1, 2)
                },
                new RectInt[]
                {
                    new RectInt(0, 2, 3, 1),
                },
                new RectInt[]
                {
                    new RectInt(2, 0, 1, 3),
                    new RectInt(0, 2, 2, 1)
                },
                new RectInt[] { },
                new RectInt[] { },
                new RectInt[]
                {
                    new RectInt(-1, -1, 3, 1),
                    new RectInt(-1, 0, 1, 2)
                },
                new RectInt[]
                {
                    new RectInt(-1, -1, 5, 1),
                    new RectInt(-1, 0, 1, 4),
                    new RectInt(3, 0, 1, 4),
                    new RectInt(0, 3, 3, 1)
                },
                new RectInt[] { },
            };

            for (int i = 0; i < rects.Length; i++)
            {
                var rect = rects[i];
                var subtract = subtracts[i];
                var result = results[i];

                var rectangles = RectIntTool.Subtract(rect, subtract);

                for (int j = 0; j < Mathf.Min(result.Length, rectangles.Count); j++)
                {
                    Assert.AreEqual(rectangles[j], result[j]);
                }

                Assert.AreEqual(result.Length, rectangles.Count);
            }
        }
    }
}
