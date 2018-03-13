using LibNoise;
using LibNoise.Generator;

public class NoiseGenerator
{
    public static float[,] Generate(int width, int height, float sizeX, float sizeY, float offsetX, float offsetY)
    {
        Perlin myPerlin = new Perlin();

        ModuleBase myModule = myPerlin;

        Noise2D heightMap;

        heightMap = new Noise2D(width, height, myModule);

        heightMap.GeneratePlanar(
            offsetX,
            offsetX + sizeX,
            offsetY,
            offsetY + sizeY
            );

        return heightMap.GetData();
    }
}