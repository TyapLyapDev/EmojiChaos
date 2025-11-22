using YG;

namespace YG
{
    public partial class SavesYG
    {
        public int CurrentLevel = 0;
    }
}

public class Saver
{
    public int CurrentLevel = YG2.saves.CurrentLevel;

    public void SaveCurrentLevel(int level)
    {
        YG2.saves.CurrentLevel = level;
        Save();
    }

    public void Save() =>
        YG2.SaveProgress();
    
}