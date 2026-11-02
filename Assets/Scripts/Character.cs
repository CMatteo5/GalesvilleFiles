using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public int characterID;
    public bool alive;
    public int trust;
    public int numberOfInteractions;
    public Sprite sprite;
}
