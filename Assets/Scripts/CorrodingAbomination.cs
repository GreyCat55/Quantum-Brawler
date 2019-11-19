using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrodingAbomination : MonoBehaviour {

    public GameObject minion;
    public int health = 100;
	// Use this for initialization
	void Start () {
        StartCoroutine(DropMinion());
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator DropMinion()
    {
        while (health > 0)
        {
            Instantiate(minion, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3);
        }
    }
}
