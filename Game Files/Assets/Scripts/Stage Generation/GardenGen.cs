using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenGen : MonoBehaviour
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

        //obtain random mushroom platforms
        GameObject lMush = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject rMush = mushrooms[Random.Range(0, mushrooms.Length)];

        //obtain random leaf platforms
        GameObject lLeaf1 = leaves[Random.Range(0, leaves.Length)];
        GameObject lLeaf2 = leaves[Random.Range(0, leaves.Length)];
        GameObject rLeaf1 = leaves[Random.Range(0, leaves.Length)];
        GameObject rLeaf2 = leaves[Random.Range(0, leaves.Length)];

        //obtain random obstacle platform
        GameObject center = obstacle[Random.Range(0, obstacle.Length)];

        //generate platforms
        leafGen(lLeaf1, lLeaf2, rLeaf1, rLeaf2);
        mushGen(lMush, rMush);
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
    void mushGen(GameObject leftM, GameObject rightM)
    {
        spawnPlat(leftM, platformV, 0);
        spawnPlat(rightM, platformV * -1, 0);
    }

    //generate the leaf platforms
    void leafGen(GameObject ll1, GameObject ll2, GameObject lr1, GameObject lr2)
    {
        spawnPlat(ll1, leafX1, leafHeight1);
        spawnPlat(ll2, leafX1 * -1, leafHeight1);
        spawnPlat(lr1, leafX2, leafHeight2);
        spawnPlat(lr2, leafX2 * -1, leafHeight2);
    }

    //spawns platform at givne coordinates
    void spawnPlat(GameObject obj, int x, int y)
    {
        obj = Instantiate(obj, new Vector2(x, y), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
