using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertGen : MonoBehaviour
{
    //platform values mainly for testing
    [SerializeField] int height1, height2;
    [SerializeField] int length1, length2;

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
        GameObject lLeaf = leaves[Random.Range(0, leaves.Length)];
        GameObject rLeaf = leaves[Random.Range(0, leaves.Length)];

        //obtain random mushroom platforms
        GameObject cMush = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject lMush = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject rMush = mushrooms[Random.Range(0, mushrooms.Length)];

        //obtain random obstacle platform
        GameObject cObs = obstacle[Random.Range(0, obstacle.Length)];
        GameObject lObs = obstacle[Random.Range(0, obstacle.Length)];
        GameObject rObs = obstacle[Random.Range(0, obstacle.Length)];

        //generate platforms
        leafGen(lLeaf, rLeaf);
        mushGen(cMush, lMush, rMush);
        obsGen(cObs, lObs, rObs);
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
    void mushGen(GameObject centerM, GameObject leftM, GameObject rightM)
    {
        spawnPlat(centerM, 0, 0);
        spawnPlat(leftM, length2 * -1, height2);
        spawnPlat(rightM, length2, height2);
    }

    //generate the leaf platforms
    void leafGen(GameObject leftL, GameObject rightL)
    {
        spawnPlat(leftL, length1 * -1, height1);
        spawnPlat(rightL, length1, height1);

    }

    //generate the obstacle platforms
    void obsGen(GameObject centerO, GameObject leftO, GameObject rightO)
    {
        spawnPlat(centerO, 0, height2);
        spawnPlat(leftO, length2 * -1, 0);
        spawnPlat(rightO, length2, 0);
    }

    //spawns platform at givne coordinates
    void spawnPlat(GameObject obj, int x, int y)
    {
        obj = Instantiate(obj, new Vector2(x, y), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
