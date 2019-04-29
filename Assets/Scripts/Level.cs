using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "New Level")]
public class Level : ScriptableObject
{
    public int levelNumber = 0;

    public int[] goals;

    public ShapeSettings shapeSettings;

    public ColourSettings colourSettings;
}
