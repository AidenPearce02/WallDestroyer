using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }
    private string fileSkinsData = "Skins";
    public Skins SkinsData { get; private set; }
    public int countSkins { get; private set; }
    public List<Sprite> SkinsHead { get; private set; }
    public List<Sprite> SkinsFull{ get; private set;}
    public enum State
    {
        BOUGHT,
        AVAILABLE,
        USED
    }
    public class Skin
    {
        public string name { get; private set; }
        public Sprite skinHead { get; private set; }
        public Sprite skinFull { get; private set; }
        public State state { get; private set; }
        public int price { get; private set; }
        public Skin(string name,Sprite skinHead,Sprite skinFull, int price, State state)
        {
            this.name = name;
            this.skinHead = skinHead;
            this.skinFull = skinFull;
            this.price = price;
            this.state = state;
        }
        public void SetState(State state) {
            this.state = state; 
        }
    }
    public Skin GetSkinByName(string name) {
        foreach (var skin in Skins)
        {
            if (skin.name == name)
            {
                return skin;
            }
        }
        return null;
    }
    public string OpenSkins { get; private set; }
    public List<Skin> Skins { get; private set; }
    public Skin CurrentSkin { get; private set; }

    public void ChangeStateBySkin(Skin skin,State state) {
        foreach (var skinTemp in Skins)
        {
            if (skinTemp == skin) {
                skinTemp.SetState(state);
            }
        }
    }
    public string SetCurrentSkin(Skin skin) {
        string usedSkin="";
        foreach (var skinTemp in Skins) {
            if (skinTemp.state == State.USED) {
                usedSkin = skinTemp.name;
                skinTemp.SetState(State.BOUGHT);
            }
        }
        CurrentSkin = skin;
        foreach (var skinTemp in Skins)
        {
            if (skinTemp == CurrentSkin)
                skinTemp.SetState(State.USED); 
        }
        return usedSkin;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        SkinsData = XmlIO.LoadXml<Skins>(fileSkinsData);
        SkinsHead =  Resources.LoadAll<Sprite>("Sprites/Skins/Head/").ToList();
        SkinsFull =  Resources.LoadAll<Sprite>("Sprites/Skins/Full/").ToList();
        Skins = new List<Skin>();
        countSkins = SkinsHead.Count;
        if (GameManager.Instance)
        {
            if (GameManager.Instance.Skins == null) {
                OpenSkins = "2";
                for(int index = 1; index < countSkins; index++)
                {
                    OpenSkins += "0";
                }
            }
            else OpenSkins = GameManager.Instance.Skins;
            int i = 0;
            foreach (Skins.Skin item in SkinsData.skins)
            {
                Sprite head=null;
                foreach (Sprite sprite in SkinsHead)
                {
                    if (sprite.name.Contains(item.name))
                    {
                        head = sprite;
                        break;
                    }
                }
                Sprite full=null;
                foreach (Sprite sprite in SkinsFull)
                {
                    if (sprite.name.Contains(item.name))
                    {
                        full = sprite;
                        break;
                    }
                }
                if (OpenSkins[i] == '0') Skins.Add(new Skin(item.name, head, full, item.price, State.AVAILABLE));
                else if (OpenSkins[i] == '1') Skins.Add(new Skin(item.name, head, full, item.price, State.BOUGHT));
                else if (OpenSkins[i] == '2')
                {
                    Skins.Add(new Skin(item.name, head, full, item.price, State.USED));
                    CurrentSkin = Skins.ToArray()[i];
                }
                i++;
            }
        }
        if (SaveSystem.LoadData() == null) SaveSystem.SaveData();
    }

    public void SaveData() {
        string temp = "";
        foreach (var skin in Skins)
        {
            if (skin.state == State.AVAILABLE) temp += '0'; 
            else if (skin.state == State.BOUGHT) temp += '1';
            else temp += '2';
        }
        OpenSkins = temp;
        GameManager.Instance.SetSkins(OpenSkins);
    }
}
