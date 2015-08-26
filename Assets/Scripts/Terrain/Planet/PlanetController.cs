using UnityEngine;
using System.Collections;

public class PlanetController : MonoBehaviour {
    public float Scale = 1f;
    public int Seed = 0;

    private byte[,,] WorldData = new byte[32, 64, 64];
    private int SizeX = 0;
    private int SizeZ = 0;

    private NoiseGenerator noise;

    void Start()
    {
        noise = new NoiseGenerator();

        if (Seed == 0)
        {
            System.Random r = new System.Random();

            Seed = r.Next(int.MaxValue);
            NoiseGenerator.Seed = Seed;
        }
    }

    public void GenerateChunk(int xpos, int zpos)
    {
        int StartX = 16 * (xpos - 1);
        int StartZ = 16 * (zpos - 1);

        for (int z=StartZ; z<StartZ+16; z++)
        {
            for (int x=StartX; x<StartX+16; x++)
            {
                double Random = noise.Noise(x, z) * Scale;
                int Height = Mathf.Clamp((int)((Random + 1) * 32), 0, 64);

                WorldData[x, Height, z] = 1;
                WorldData[x, 0, z] = 255;
            }
        }
    }

    public byte[,,] GetChunk(int xpos, int zpos)
    {
        byte[,,] Chunk = new byte[16, 64, 16];

        int StartX = 16 * (xpos - 1);
        int StartZ = 16 * (zpos - 1);

        for (int z = StartZ; z < StartZ + 16; z++)
        {
            for (int y = 0; y < WorldData.GetLength(1) - 1; y++)
            {
                for (int x = StartX; x < StartX + 16; x++)
                {
                    Chunk[x, y, z] = WorldData[x, y, z];
                }
            }
        }

        return Chunk;
    }

    /*
    void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            for (int x = 0; x < WorldData.GetLength(0)-1; x++)
            {
                for (int y = 0; y < WorldData.GetLength(1)-1; y++)
                {
                    for (int z = 0; z < WorldData.GetLength(2)-1; z++)
                    {
                        switch (WorldData[x,y,z])
                        {
                            case 0:
                                break;
                            case 1:
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(new Vector3(x, y, z), 0.1f);
                                break;
                            case 255:
                                Gizmos.color = Color.red;
                                //Gizmos.DrawSphere(new Vector3(x, y, z), 0.1f);
                                break;
                            
                        }
                    }
                }
            }
        }
    }
    */
}
