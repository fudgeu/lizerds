using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GardenGen : MonoBehaviour
{
    //platform values mainly for testing
    [SerializeField] private int offsetX, offsetY;

    //fields to add game objects to file
    [SerializeField] private GameObject drop, level, seesaw, spring, normal;

    // Start is called before the first frame update
    void Start()
    {
        //declaring the arrays for generation
        GameObject[] mushrooms = new GameObject[5];
        mushrooms[0] = drop;
        mushrooms[1] = level;
        mushrooms[2] = seesaw;
        mushrooms[3] = spring;
        mushrooms[4] = normal;

        //obtain random mushroom platforms
        GameObject lMush = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject cMush = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject rMush = mushrooms[Random.Range(0, mushrooms.Length)];

        //generate platforms
        mushGen(lMush, cMush, rMush);
    }

    //generate the mushroom platforms
    void mushGen(GameObject leftM, GameObject cMush, GameObject rightM)
    {
        spawnPlat(leftM, offsetX, offsetY);
        spawnPlat(leftM, 0, offsetY + 2);
        spawnPlat(rightM, offsetX * -1, offsetY);
    }

    //spawns platform at givne coordinates
    void spawnPlat(GameObject obj, int x, int y)
    {
        obj = Instantiate(obj, new Vector2(x, y), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
