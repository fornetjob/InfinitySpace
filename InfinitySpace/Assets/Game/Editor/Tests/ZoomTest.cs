using Assets.Game.UI.Controls;

using NUnit.Framework;

using UnityEngine;

namespace Assets.Game.Editor.Tests
{
    public class ZoomTest
    {
       /// <summary>
       /// Проверка зума
       /// </summary>
        [Test]
        public void VisibleRectCheck()
        {
            var rects = new RectInt[]
            {
                ZoomControl.GetVisibleRect(Vector2Int.zero, 200)
            };

            var checkRects = new RectInt[]
            {
                new RectInt(-100, -100, 200, 200),
            };

            Assert.AreEqual(rects.Length, checkRects.Length);

            for (int i = 0; i < rects.Length; i++)
            {
                Assert.AreEqual(rects[i], checkRects[i]);
            }
        }
    }
}