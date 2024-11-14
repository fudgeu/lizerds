using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesignController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<GameObject> sprites = new List<GameObject>();

    [Header("Configuration")]
    public Color bodyColor = Color.white;
    public Color jawColor = Color.white;
    public Color eyeColor = Color.white;

    private void OnValidate()
    {
        foreach (GameObject obj in sprites)
        {
            if (obj.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
            {
                switch (obj.tag)
                {
                    case "Color1":
                        sprite.color = bodyColor; break;
                    case "Color2":
                        sprite.color = jawColor; break;
                    case "Color3":
                        sprite.color = eyeColor; break;
                    default:
                        break;
                }
            }
        }
    }
}
