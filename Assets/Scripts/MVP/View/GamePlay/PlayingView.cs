using UnityEngine;

public class PlayingView : View
{
    public override void Initialize()
    {

    }

    public void Start()
    {
        EventSystem.Instance.TriggerEvent(StringConstant.EVENT.ON_SCENE_LOADED);
        UiManager.Instance.RegisterStartingView(this);
    }
}
