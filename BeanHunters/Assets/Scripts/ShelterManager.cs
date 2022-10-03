using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShelterManager : MonoBehaviour
{


    private Tilemap map;
    public Tile shelterTile;
    public int shelterInverseDensity = 10;
    public int seed = 0;

    private Camera cam;

    private Vector3 shelterOffset;

    void Start()
    {
        map = GetComponent<Tilemap>();
        cam = FindObjectOfType<Camera>();
        shelterOffset = (map.CellToWorld((Vector3Int)Vector2Int.one) - map.CellToWorld(Vector3Int.zero)) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = OrthographicBounds();
        Vector3Int bottomLeft = map.WorldToCell(new Vector3(bounds.center.x - bounds.size.x / 2, bounds.center.y - bounds.size.y / 2, 0));
        Vector3Int topRight = map.WorldToCell(new Vector3(bounds.center.x + bounds.size.x / 2, bounds.center.y + bounds.size.y / 2, 0));

        Vector2Int dBottomLeft = getShelterDistrict(bottomLeft);
        Vector2Int dTopRight = getShelterDistrict(topRight) + Vector2Int.one;

        for (int x = dBottomLeft.x; x <= dTopRight.x; x++)
        {
            for (int y = dBottomLeft.y; y <= dTopRight.y; y++)
            {
                if(x != 0 || y != 0)
                {
                    map.SetTile(getShelterPosition(x, y), shelterTile);
                }
            }
        }
    }

    public bool isOverShelter(Vector3 position)
    {
        return (getShelterDistrict(position) != Vector2Int.zero
             && map.WorldToCell(position) == getShelterPosition(getShelterDistrict(position)));
    }

    public Vector3Int getShelterPosition(Vector2Int district)
    {
        // x ^ 2 * y, 2 * y ^ 2
        return getShelterPosition(district.x, district.y);
        /*return new Vector3Int(district.x * shelterInverseDensity + Mathf.FloorToInt(rand(district.x ^ 2          * district.y)*shelterInverseDensity), 
                              district.y * shelterInverseDensity + Mathf.FloorToInt(rand(2          * district.y ^          2)*shelterInverseDensity),
                              0);*/
    }

    public Vector3Int getShelterPosition(int districtX, int districtY)
    {
        // x ^ 2 * y, 2 * y ^ 2
        return new Vector3Int(districtX * shelterInverseDensity - shelterInverseDensity/2 + Mathf.FloorToInt(rand(districtX ^ 2 * districtY, 2 * districtY ^ 2) * shelterInverseDensity),
                              districtY * shelterInverseDensity - shelterInverseDensity/2 + Mathf.FloorToInt(rand((districtX ^ 2 * districtY) + 23.43f, (2 * districtY ^ 2) * 12.424f) * shelterInverseDensity),
                              0);
    }

    public Vector3 closestShelter(Vector3 position)
    {
        Vector2Int district = getShelterDistrict(position);
        Vector3 closest = new Vector3();
        float closestDistance = float.MaxValue;
        Vector3 candidate;
        for(int x = district.x-1; x <= district.x+1; x++)
        {
            for(int y = district.y-1; y <= district.y+1; y++)
            {
                if (x == 0 && y == 0) continue;
                candidate = map.CellToWorld(getShelterPosition(x, y)) + shelterOffset;
                if((candidate - position).sqrMagnitude < closestDistance)
                {
                    closest = candidate;
                    closestDistance = (candidate - position).sqrMagnitude;
                }
            }
        }
        return closest;
    }

    public Vector2Int getShelterDistrict(Vector3 position)
    {
        Vector3Int cell = map.WorldToCell(position);
        return getShelterDistrict(cell);
        //return new Vector2Int(Mathf.FloorToInt(position.x / shelterInverseDensity), Mathf.FloorToInt(position.y / shelterInverseDensity));
    }

    public Vector2Int getShelterDistrict(Vector3Int cell)
    {
        return getShelterDistrict(cell.x, cell.y);
        //return new Vector2Int(Mathf.FloorToInt((float)cell.x / shelterInverseDensity), Mathf.FloorToInt((float)cell.y / shelterInverseDensity));
    }

    public Vector2Int getShelterDistrict(int cellX, int cellY)
    {
        return new Vector2Int(Mathf.FloorToInt(((float)cellX + shelterInverseDensity/2) / shelterInverseDensity), 
                              Mathf.FloorToInt(((float)cellY + shelterInverseDensity/2) / shelterInverseDensity));
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
        return Mathf.Abs((float)(Mathf.Sin((float)(x * 12.9898 + y * 78.233 + seed)) * 43758.5453) % 1);
    }
}
