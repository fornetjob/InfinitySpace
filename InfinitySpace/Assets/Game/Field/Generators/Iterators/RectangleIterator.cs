using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Field.Generators.Iterators
{
    public class RectangleIterator : IEnumerator<Vector2Int>
    {
        private IEnumerator<RectInt>
            _rects;

        private RectInt.PositionEnumerator?
            _current;

        public int Lenght;

        public Vector2Int Current
        {
            get
            {
                if (_current == null)
                {
                    return Vector2Int.zero;
                }

                return _current.Value.Current;
            }
        }

        public void Dispose()
        {
            _rects = null;
            _current = null;
        }

        public bool MoveNext()
        {
            if (_current == null)
            {
                if (_rects.MoveNext() == false)
                {
                    return false;
                }

                _current = _rects.Current.allPositionsWithin;
            }

            return _current.Value.MoveNext();
        }

        public void Reset(List<RectInt> rects)
        {
            _rects = rects.GetEnumerator();

            Lenght = 0;

            for (int i = 0; i < rects.Count; i++)
            {
                var rect = rects[i];

                Lenght += rect.width * rect.height;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
