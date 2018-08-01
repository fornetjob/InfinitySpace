using Assets.Game.Editor.Tests.DataContracts;

using NUnit.Framework;

using System.Text;

using UnityEngine;

namespace Assets.Game.Editor.Tests
{
    public class ShaderTest
    {
        /// <summary>
        /// Контракт для тестов
        /// </summary>
        private class ShaderTestInfo
        {
            public int Size;
            public int PosX;
            public int PosY;
        }

        /// <summary>
        /// Проверка сортировки
        /// </summary>
        [Test]
        public void SortCheck()
        {
            Assert.IsTrue(SystemInfo.supportsComputeShaders);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            SortCells sort = new SortCells();
            var results = sort.Sort(10000);

            StringBuilder builder = new StringBuilder();

            foreach (var current in results)
            {
                builder.Append(current.X + ", " + current.Y + ": " + current.Rating);
                builder.AppendLine();
            }

            timer.Stop();

            Debug.Log(builder.ToString() + " " + timer.ElapsedMilliseconds);
        }
    }
}
