using System;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public class GridMap<TGridObject>
    {
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        private readonly int _width, _height;
        private readonly float _cellSize;
        private readonly Vector3 _originPosition;
        private readonly TGridObject[,] _grid;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        public GridMap(int width, int height, float cellSize, Vector3 originPosition,
            Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _grid = new TGridObject[width, height];
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    TGridObject @object = createGridObject(this, x, y);
                    _grid[x, y] = @object;
                }
            }
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * _cellSize + _originPosition;
        }

        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);

        }

        public void SetValue(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _grid[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { X = x, Y = y });
        }

        public TGridObject GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _grid[x, y];
            }
            else
            {
                return default;
            }
        }
    }
}
