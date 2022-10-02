using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTiles : MonoBehaviour
{
    private Tilemap map;
    public Tile[] tiles;

    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = OrthographicBounds();
        Vector3Int bottomLeft = map.WorldToCell(new Vector3(bounds.center.x - bounds.size.x / 2, bounds.center.y - bounds.size.y / 2, 0));
        Vector3Int topRight = map.WorldToCell(new Vector3(bounds.center.x + bounds.size.x / 2, bounds.center.y + bounds.size.y / 2, 0));

        Vector3Int pos = new Vector3Int();

        for(int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                pos.x = x;
                pos.y = y;
                int randomIndex = Mathf.FloorToInt(rand(x ^ 2 * y, 2 * y ^ 2) * tiles.Length);
                map.SetTile(pos, tiles[randomIndex]);
            }
        }
    }

    private Bounds OrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new Bounds(
            cam.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    private float rand(float x, float y)
    {
        return Mathf.Abs((float)(Mathf.Sin((float)(x* 12.9898 + y*78.233)) * 43758.5453) % 1);
    }
}
