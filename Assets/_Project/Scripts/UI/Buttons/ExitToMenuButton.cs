using YG;

public class ExitToMenuButton : ButtonClickHandler<ExitToMenuButton> 
{
    protected override void OnClick()
    {
        base.OnClick();

        if (YG2.saves.SavesData.LevelProgress >= 2)
            YG2.ReviewShow();

        if (YG2.saves.SavesData.LevelProgress >= 4)
            YG2.GameLabelShowDialog();
    }
}