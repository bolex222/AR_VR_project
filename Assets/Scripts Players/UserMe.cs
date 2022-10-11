using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public int lifeNumber = 10;
    private bool isDead = false;
    public GameObject spawnLocations;
    GameObject player;
    public List<string> allTags = new List<string>();
    public TextMeshProUGUI displayLife;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnTriggerEnter(Collider bullet)
    {
        foreach(string tag in allTags)
        {
            if (bullet.tag == tag)
            {
                print(bullet.transform.parent.gameObject.layer);
                if(bullet.transform.parent.gameObject.layer == 6)
                {
                    lifeNumber = lifeNumber - 1;
                    displayLife.text = lifeNumber.ToString();
                }
                


            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (lifeNumber == 0) { isDead = true; } else { isDead = false; }
        if (isDead)
        {
            transform.position = spawnLocations.transform.position;
            lifeNumber = 10;
            displayLife.text = lifeNumber.ToString();
            isDead = false;
        }
    }
}
