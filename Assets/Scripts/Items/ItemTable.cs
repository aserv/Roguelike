using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Reflection;

public class ItemTable {
    private class DropList {
        private struct DropRateElement {
            public BaseItem item;
            public int rate;
            public int sum;
        };
        private int total = 0;
        private int count = 0;
        private DropRateElement[] items = null;
        private void Resize(int size = 0) {
            if (items == null) {
                items = new DropRateElement[8];
            } else {
                if (size == 0) { size = items.Length * 2; } else if (size <= items.Length) { return; }
                DropRateElement[] nitems = new DropRateElement[size];
                items.CopyTo(nitems, 0);
                items = nitems;
            }
        }
        public void Add(BaseItem item, int rate) {
            if (items == null || items.Length == count) { Resize(); }
            total += rate;
            items[count++] = new DropRateElement() { item = item, rate = rate, sum = total };
        }
        public BaseItem RandomDrop(int val = -1) {
            if (val == -1) {
                val = Random.Range(0, total);
            }
            Debug.Log(val);
            int s = 0, e = count;
            while (s < e) {
                int mid = (s + e) / 2;
                if (items[mid].sum < val) {
                    s = mid + 1;
                } else if (items[mid].sum - items[mid].rate < val) {
                    return items[mid].item;
                } else {
                    e = mid - 1;
                }
            }
            return items[s].item;
        }
    };
    private ItemTable() { }
    public ItemTable(int tiers) {
        droptiers = new DropList[tiers];
        for (int i = 0; i < tiers; i++) {
            droptiers[i] = new DropList();
        }
        itemlist = new Dictionary<string, BaseItem>();
    }
    private Dictionary<string, BaseItem> itemlist;
    private DropList[] droptiers;
    public BaseItem this[string name] {
        get { return itemlist[name]; }
    }
    public void AddItem(BaseItem item, int tier, int rate) {
        itemlist.Add(item.Name, item);
        droptiers[tier].Add(item, rate);
    }
    public BaseItem RandomDrop(int mintier, int maxtier, float promote = 0.25f) {
        mintier = Mathf.Min(mintier, droptiers.Length - 1);
        maxtier = Mathf.Min(maxtier, droptiers.Length - 1);
        int t = mintier;
        while (t < maxtier && Random.Range(0, 1) < promote) { ++t; }
        if (t < 0) { return null; }
        return droptiers[t].RandomDrop();
    }
    private static bool ReadTo(XmlReader reader, XmlNodeType type, string name) {
        while (reader.Read()) {
            if (reader.NodeType == type && reader.Name == name)
                return true;
        }
        return false;
    }
    private static BaseItem LoadItem(XmlReader reader) {
        reader.MoveToContent();
        ArrayList args = new ArrayList();
        string typeName = reader.GetAttribute("name");
        
        try {
            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "type") break;
                if (reader.NodeType == XmlNodeType.Element) {
                    string n = reader.Name;
                    if (n == "name") {
                        args.Add(reader.ReadElementContentAsString());
                    } else if (n == "icon") {
                        args.Add(Resources.Load(reader.ReadElementContentAsString(), typeof(Sprite)) as Sprite);
                    } else if (n == "prefab") {
                        args.Add(Resources.Load(reader.ReadElementContentAsString(), typeof(GameObject)) as GameObject);
                    } else {
                        Debug.LogFormat("Ignoring type argument {0}", reader.Name);
                    }
                }
            }
            System.Type runtimeType = Assembly.GetExecutingAssembly().GetType(typeName);

            BaseItem it = System.Activator.CreateInstance(runtimeType, args.ToArray()) as BaseItem;
            return it;
        } catch (System.Exception e) {
            Debug.Log(e.Message);
            return null;
        }
    }

    public static ItemTable Load(string resource) {
        ItemTable table = new ItemTable();
        table.itemlist = new Dictionary<string, BaseItem>();
        XmlReader reader = XmlReader.Create(new FileStream(resource, FileMode.Open));
        reader.MoveToContent();
        if (!reader.ReadToDescendant("items")) {
            Debug.Log("XML Has no items");
            return null;
        }

        if (reader.ReadToDescendant("item")) {
            do {
                BaseItem it = LoadItem(reader.ReadSubtree());
                if (it != null) {
                    table.itemlist.Add(it.Name, it);
                } else {
                    Debug.Log("Failed to Load Item");
                }
            } while (reader.ReadToNextSibling("item"));
        }


        if (!reader.ReadToNextSibling("droptable")) {
            Debug.Log("XML Has no droptable");
            return null;
        }
        int tiers = 0;
        if (System.Int32.TryParse(reader.GetAttribute("tiers"), out tiers)) {
            table.droptiers = new DropList[tiers];
            for (int i = 0; i < tiers; i++) table.droptiers[i] = new DropList();
        } else {
            Debug.Log("Droplist has no tiers attribute");
            return null;
        }
        
        if (reader.ReadToDescendant("drop")) {
            do {
                string name;
                int t, r;
                name = reader.GetAttribute("item");
                if (!System.Int32.TryParse(reader.GetAttribute("tier"), out t) ||
                    !System.Int32.TryParse(reader.GetAttribute("rate"), out r)) {
                    continue;
                }
                if (t >= tiers) {
                    Debug.LogFormat("Tier {0} for is out of range {1}", t, name);
                    continue;
                }
                BaseItem it = table.itemlist[name];
                if (it == null) {
                    Debug.LogFormat("Item {0} not found", name);
                    continue;
                }
                table.droptiers[t].Add(it, r);
            } while (reader.ReadToNextSibling("drop"));
        }

        return table;
    }
}