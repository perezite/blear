using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// manages a list of black brick removers, such that the are activated only when all removers have been hit
public class BlackBrickRemoverManager : MonoBehaviour
{
    [Tooltip("Black bricks which are removed when all removers have been hit")]
    public List<BlackBrick> BlackBricks = new List<BlackBrick>();

    [Tooltip("The black bricks disappear as soon as all removers in this list have been destroyed")]
    public List<Brick> Removers = new List<Brick>();

    // list of removers with destruction pending or destruction completed
    private List<int> destructionPendingRemovers = new List<int>();

    // Use this for initialization
    private void Start()
    {
        Removers.ForEach(x => x.DestructionPending += OnRemoverDestructionPending);
    }
    
    private void OnRemoverDestructionPending(Object sender)
    {
        destructionPendingRemovers.Add(sender.GetInstanceID());
        destructionPendingRemovers = destructionPendingRemovers.Distinct().ToList();

        if (destructionPendingRemovers.Count == Removers.Count)
        {
            BlackBricks.ForEach(x => StartCoroutine(x.Destroy()));
        }
    }
}
