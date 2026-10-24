using System;
using System.Collections.Generic;

namespace UAM3D.types;

/// <summary>
/// A permutable, three-dimensional matrix.
/// </summary>
/// <typeparam name="T">The target type.</typeparam>
public class Sparse3DMatrix<T>
{
    private Dictionary<Tuple<int, int, int>, T> _data = new();

    public T this[int x, int y, int z]
    {
        get
        {
            var key = new Tuple<int, int, int>(x, y, z);
            return _data.GetValueOrDefault(key);
        }
        set
        {
            var key = new Tuple<int, int, int>(x, y, z);
            if (EqualityComparer<T>.Default.Equals(value, default)) _data.Remove(key);
            else _data[key] = value;
        }
    }
    
    /// <summary>
    /// Gets the six cells that share a face with the cell at <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The coordinates of the cell whose neighbors should be returned.</param>
    /// <returns>A sparse matrix containing the neighboring cells.</returns>
    public Sparse3DMatrix<T> GetNeighbors(int[] key)
    {
        return GetNeighbors(key[0], key[1], key[2]);
    }

    /// <summary>
    /// Gets the six cells that share a face with the cell at the specified coordinates.
    /// </summary>
    public Sparse3DMatrix<T> GetNeighbors(int x, int y, int z) =>
        new()
        {
            [x - 1, y, z] = this[x - 1, y, z],
            [x + 1, y, z] = this[x + 1, y, z],
            [x, y - 1, z] = this[x, y - 1, z],
            [x, y + 1, z] = this[x, y + 1, z],
            [x, y, z - 1] = this[x, y, z - 1],
            [x, y, z + 1] = this[x, y, z + 1]
        };
}