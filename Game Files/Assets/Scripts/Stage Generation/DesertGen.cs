using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertGen : MonoBehaviour
{
    //platform values mainly for testing
    [SerializeField] int platformV, leafHeight1, leafHeight2, leafX1, leafX2;

    //fields to add game objects to file
    [SerializeField] GameObject drop, lever, slide;
    [SerializeField] GameObject puff, seesaw, spring, sway;
    [SerializeField] GameObject test1, test2, test3;

    // Start is called before the first frame update
    void Start()
    {
        //declaring the arrays for generation
        GameObject[] leaves = new GameObject[5];
        GameObject[] mushrooms = new GameObject[5];
        GameObject[] obstacle = new GameObject[5];

        //populate the arrays
        populate(leaves, mushrooms, obstacle);

        //obtain random leaf platforms
        GameObject leftP = mushrooms[Random.Range(0, leaves.Length)];
        GameObject rightP = mushrooms[Random.Range(0, leaves.Length)];

        //obtain random mushroom platforms
        GameObject leafl1 = leaves[Random.Range(0, leaves.Length)];
        GameObject leafl2 = leaves[Random.Range(0, leaves.Length)];
        GameObject leafr1 = leaves[Random.Range(0, leaves.Length)];
        GameObject leafr2 = leaves[Random.Range(0, leaves.Length)];

        //obtain random obstacle platform
        GameObject center = obstacle[Random.Range(0, leaves.Length)];

        //generate platforms
        leafGen(leafl1);
        mushGen(leftP);
        spawnPlat(center, 0, 0);
    }

    //function to populate the object arrays
    void populate(GameObject[] leaves, GameObject[] mushrooms, GameObject[] obstacle)
    {
        //populate leaves
        leaves[0] = drop;
        leaves[1] = lever;
        leaves[2] = sway;
        leaves[3] = drop;
        leaves[4] = sway;

        //populate mushrooms
        mushrooms[0] = puff;
        mushrooms[1] = seesaw;
        mushrooms[2] = sway;
        mushrooms[3] = sway;
        mushrooms[4] = spring;

        //populate obstacles
        obstacle[0] = test1;
        obstacle[1] = test2;
        obstacle[2] = test3;
        obstacle[3] = test1;
        obstacle[4] = test2;
    }

    //generate the mushroom platforms
    void mushGen(GameObject m)
    {
        spawnPlat(m, platformV, 0);
        spawnPlat(m, platformV * -1, 0);
    }

    //generate the leaf platforms
    void leafGen(GameObject l)
    {
        spawnPlat(l, leafX1, leafHeight1);
        spawnPlat(l, leafX1 * -1, leafHeight1);
        spawnPlat(l, leafX2, leafHeight2);
        spawnPlat(l, leafX2 * -1, leafHeight2);
    }

    //spawns platform at givne coordinates
    void spawnPlat(GameObject obj, int x, int y)
    {
        obj = Instantiate(obj, new Vector2(x, y), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
