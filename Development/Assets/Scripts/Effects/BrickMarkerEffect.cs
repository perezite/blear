using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BrickEffectsGrouping = System.Collections.Generic.Dictionary<float, System.Collections.Generic.List<BrickEffects>>;

public class BrickMarkerEffect : MonoBehaviour
{                          
    [Tooltip("Row marker prefab")]
    public GameObject RowMarkerPrefab;

    [Tooltip("Column marker prefab")]
    public GameObject ColumnMarkerPrefab;

    // position offset to compensate for offset in sprite image
    private const float VerticalEffectOffset = 0.22f;

    // list of brick effects
    private List<BrickEffects> brickEffects = new List<BrickEffects>();
    
    // vertical grouping
    private BrickEffectsGrouping effectsGroupedIntoRows = new BrickEffectsGrouping();

    // horizontal brick effects grouping
    private BrickEffectsGrouping effectsGroupedIntoColumns = new BrickEffectsGrouping();

    // vertical brick marker clones
    private Dictionary<float, SpriteRenderer> rowMarkers = new Dictionary<float, SpriteRenderer>();

    // horizontal brick markerclones
    private Dictionary<float, SpriteRenderer> columnMarkers = new Dictionary<float, SpriteRenderer>();

    private void Awake()
    {
        brickEffects = CollectBrickEffects();
        effectsGroupedIntoRows = CalculateBrickEffectsGrouping(true, brickEffects);
        effectsGroupedIntoColumns = CalculateBrickEffectsGrouping(false, brickEffects);

        // register brick actions
        var bricks = brickEffects.Select(x => x.transform.GetComponent<Brick>()).Where(x => x != null).ToList();
        bricks.ForEach(x => x.DestructionPending += OnBrickDestructionPending);
    }

    private void Start()
    {
        // instantiate row markers
        foreach (var group in effectsGroupedIntoRows)
        {
            var position = new Vector2(0f, group.Key);
            var clone = (GameObject)Instantiate(RowMarkerPrefab, position, Quaternion.identity);
            rowMarkers.Add(group.Key, clone.GetComponent<SpriteRenderer>());
        }

        // instantiate column markers
        foreach (var group in effectsGroupedIntoColumns)
        {
            var position = new Vector2(group.Key, VerticalEffectOffset);
            var clone = (GameObject)Instantiate(ColumnMarkerPrefab, position, Quaternion.identity);
            columnMarkers.Add(group.Key, clone.GetComponent<SpriteRenderer>());
        }
    }

    private List<BrickEffects> CollectBrickEffects()
    {
        var result = FindObjectsOfType(typeof(BrickEffects)).Select(x => (BrickEffects)x)
            .Where(x => x.ShowMarker).ToList();
        return result;
    }

    private BrickEffectsGrouping CalculateBrickEffectsGrouping(bool groupIntoRows, List<BrickEffects> effects)
    {
        var grouping = new BrickEffectsGrouping();
        var effectsAlreadyGrouped = new List<BrickEffects>();

        foreach (var effect in effects)
        {
            // skip if this element was already part of a group
            if (effectsAlreadyGrouped.Contains(effect))
            {
                continue;
            }

            // determine the grouping criterion
            Func<BrickEffects, bool> groupingExpression;
            if (groupIntoRows)
            {
                groupingExpression = e => Mathf.Approximately(effect.transform.position.y, e.transform.position.y);
            }
            else
            {
                groupingExpression = e => Mathf.Approximately(effect.transform.position.x, e.transform.position.x);
            }

            // apply grouping
            var currentGroup = effects.Where(groupingExpression).ToList();
            var groupKey = groupIntoRows ? effect.transform.position.y : effect.transform.position.x;
            grouping.Add(groupKey, currentGroup);
            effectsAlreadyGrouped.AddRange(currentGroup);
        }

        return grouping;
    }

    private void OnBrickDestructionPending(UnityEngine.Object sender)
    {
        var gameObject = (GameObject)sender;
        var brickEffect = gameObject.GetComponent<BrickEffects>();

        if (brickEffect)
        {
            // recalculate grouping
            brickEffects.Remove(brickEffect);
            effectsGroupedIntoRows = CalculateBrickEffectsGrouping(true, brickEffects);
            effectsGroupedIntoColumns = CalculateBrickEffectsGrouping(false, brickEffects);

            // update the markers
            var rowMarkersToDelete = rowMarkers.Where(rm => !effectsGroupedIntoRows.Any(e => Mathf.Approximately(e.Key, rm.Key))).ToList();
            rowMarkersToDelete.ForEach(x => x.Value.enabled = false);
            var columnMarkersToDelete = columnMarkers.Where(cm => !effectsGroupedIntoColumns.Any(e => Mathf.Approximately(e.Key, cm.Key))).ToList();
            columnMarkersToDelete.ForEach(x => x.Value.enabled = false);
        }
    }
}
