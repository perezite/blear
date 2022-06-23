using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuElement : MonoBehaviour
{
    public void OnClick()
    {
        StartCoroutine(OnClickHandler());
    }

    // set interactable
    public virtual void SetInteractable(bool isInteractable)
    {
        GetComponents<Selectable>().ToList().ForEach(x => x.interactable = isInteractable);
    }

    // click event handler
    protected virtual IEnumerator OnClickHandler()
    {
        yield return null;
    }
}