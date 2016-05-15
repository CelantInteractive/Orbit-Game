using UnityEngine;
using System.Collections;

namespace Orbit.Terrain.Planet
{
    public class Block
    {
		public struct TextureTile { public int x; public int y; }

		public const float TILE_SIZE = 1.0f;

		public enum Direction
		{
			North,
			East,
			South,
			West,
			Up,
			Down,
		}

        public Block()
        {

        }

        public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
        {

			meshData.UseRenderDataForCol = true;

			if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down))
			{
				meshData = FaceDataUp(chunk, x, y, z, meshData);
			}

			if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up))
			{
				meshData = FaceDataDown(chunk, x, y, z, meshData);
			}

			if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South))
			{
				meshData = FaceDataNorth(chunk, x, y, z, meshData);
			}

			if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North))
			{
				meshData = FaceDataSouth(chunk, x, y, z, meshData);
			}

			if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West))
			{
				meshData = FaceDataEast(chunk, x, y, z, meshData);
			}

			if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East))
			{
				meshData = FaceDataWest(chunk, x, y, z, meshData);
			}

			return meshData;
        }

        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.North));
            return meshData;
        }

        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
			meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.East));
            return meshData;
        }

        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.South));
            return meshData;
        }

        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
			meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.West));
            return meshData;
        }

        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
			meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.Up));
            return meshData;
        }

        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
			meshData.AddQuadTriangles();

			meshData.UV.AddRange (FaceUVs (Direction.Down));
            return meshData;
        }

		public virtual TextureTile TexturePosition(Direction direction)
		{
			TextureTile tile = new TextureTile ();
			tile.x = 0;
			tile.y = 0;

			return tile;
		}

		public virtual Vector2[] FaceUVs(Direction direction)
		{
			Vector2[] UVs = new Vector2[4];
			TextureTile tilePos = TexturePosition (direction);
			UVs [0] = new Vector2 (TILE_SIZE * tilePos.x + TILE_SIZE, TILE_SIZE * tilePos.y);
			UVs [1] = new Vector2 (TILE_SIZE * tilePos.x + TILE_SIZE, TILE_SIZE * tilePos.y + TILE_SIZE);
			UVs [2] = new Vector2 (TILE_SIZE * tilePos.x, TILE_SIZE * tilePos.y + TILE_SIZE);
			UVs [3] = new Vector2 (TILE_SIZE * tilePos.x, TILE_SIZE * tilePos.y);
			return UVs;
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
}