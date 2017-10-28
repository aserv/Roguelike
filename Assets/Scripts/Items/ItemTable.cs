using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
