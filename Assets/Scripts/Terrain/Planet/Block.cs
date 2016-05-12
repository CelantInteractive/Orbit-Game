using UnityEngine;
using System.Collections;

public enum Direction
{
    North,
    East,
    South,
    West,
    Up,
    Down,
}

public class Block
{

    public Block()
    {

    }

    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East))
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        return meshData;
    }

    protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.Verticies.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.Verticies.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddQuadTriangles();
        return meshData;
    }

    public virtual bool IsSolid(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return true;
            case Direction.East:
                return true;
            case Direction.South:
                return true;
            case Direction.West:
                return true;
            case Direction.Up:
                return true;
            case Direction.Down:
                return true;
        }

        return false;
    }
}
