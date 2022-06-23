using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtraDropper : MonoBehaviour
{
    [Tooltip("Prefabs of available extras")]
    public List<Extra> ExtraPrefabs = new List<Extra>();

    [Tooltip("Minimal and maximal scale factor")]
    public Vector2 ScaleFactorRange = Vector2.one;

    [Tooltip("Activate if only one extra should be dropped")]
    public bool DropOneShot;

    // the collider
    private Collider2D coll;

    // has already one extra been dropped
    private bool isExtraDropped = false;

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        bool canDrop = !DropOneShot || isExtraDropped == false;

        if (canDrop && other.transform.tag == "Ball" && ExtraPrefabs.Any())
        {
            isExtraDropped = true;

            // determine spawn position
            var horizontalSpawnPos = Random.Range(transform.position.x - coll.bounds.extents.x, transform.position.x + coll.bounds.extents.x);
            var spawnPosition = new Vector2(horizontalSpawnPos, transform.position.y - coll.bounds.extents.y);
            var scale = Random.Range(ScaleFactorRange.x, ScaleFactorRange.y);

            // clone randomly out of the provided list of extras
            var extraPrefab = ExtraPrefabs.OrderBy(x => Random.Range(int.MinValue, int.MaxValue)).First();
            var clone = (GameObject)Instantiate(extraPrefab.gameObject, spawnPosition, Quaternion.identity);
            clone.transform.localScale *= scale;
        }
    }

    // Use this for initialization
    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }
}
