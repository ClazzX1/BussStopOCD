using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform spawnPoint;

    private int stage = 1;
    private List<GameObject> characters;

	void Start ()
    {
        StageStart();
    }
	
	void Update ()
    {
	
	}

    public void StageStart()
    {
        int characterCount = stage * 20;
        for (int i = 0; i < characterCount; ++i)
        {
            Vector3 position = spawnPoint.position;
            float offsetX = (float)((i + 1) / 2) * 1.0f;
            position.x += (i % 2 == 0 ? -offsetX : offsetX);
            
            AddCharacter(position);
        }
    }

    public void AddCharacter(Vector3 position)
    {
        GameObject newObject = (GameObject)Instantiate(Resources.Load("Character"));
        newObject.transform.parent = transform;
        newObject.transform.position = position;
        characters.Add(newObject);
    }

    public void StageComplete()
    {
        ++stage;
    }
}
