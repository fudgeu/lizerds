using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampGen : MonoBehaviour
{
    //platform values mainly for testing
    [SerializeField] int height1, height2, height3;
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
        GameObject lMush1 = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject lMush2 = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject rMush1 = mushrooms[Random.Range(0, mushrooms.Length)];
        GameObject rMush2 = mushrooms[Random.Range(0, mushrooms.Length)];

        //obtain random obstacle platform
        GameObject lObs = obstacle[Random.Range(0, obstacle.Length)];
        GameObject rObs = obstacle[Random.Range(0, obstacle.Length)];

        //generate platforms
        leafGen(lLeaf, rLeaf);
        mushGen(lMush1, lMush2, rMush1, rMush2);
        obsGen(lObs, rObs);
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
    void mushGen(GameObject leftM1, GameObject leftM2, GameObject rightM1, GameObject rightM2)
    {
        spawnPlat(leftM1, length2 * -1, 0);
        spawnPlat(leftM2, length2 * -1, height2);
        spawnPlat(rightM1, length2, 0);
        spawnPlat(rightM2, length2, height2);
    }

    //generate the leaf platforms
    void leafGen(GameObject leftL, GameObject rightL)
    {
        spawnPlat(leftL, length1 * -1, height3);
        spawnPlat(rightL, length1, height3);

    }

    //generate the obstacle platforms
    void obsGen(GameObject leftO, GameObject rightO)
    {
        spawnPlat(leftO, length1 * -1, height1);
        spawnPlat(rightO, length1, height1);
    }

    //spawns platform at givne coordinates
    void spawnPlat(GameObject obj, int x, int y)
    {
        obj = Instantiate(obj, new Vector2(x, y), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
