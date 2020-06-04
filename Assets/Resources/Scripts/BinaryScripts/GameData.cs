[System.Serializable]
public class GameData
{
    public int money;
    public int lastOpenedLevel;
    public float record;
    public string perks;
    public string skins;

    public GameData() {
        money = GameManager.Instance.Money;
        lastOpenedLevel = GameManager.Instance.LastOpenedLevel;
        record = GameManager.Instance.Record;
        perks = PerkManager.Instance.OpenPerks;
        skins = SkinManager.Instance.OpenSkins;
    }
}
