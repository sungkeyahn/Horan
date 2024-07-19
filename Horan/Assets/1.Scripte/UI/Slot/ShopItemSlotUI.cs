using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlotUI : BaseUI
{
    bool isInit;
    enum Components { Image_ProductICon, Text_PriceValue, Button_ShopSlot, Text_ShopSlotBtnName }
    public List<ItemSlotUI> itemslots = new List<ItemSlotUI>();

    AudioSource Sound_Buy;
    AudioSource Sound_Sell;

    int itemID;
    float price;

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ShopSlot), OnPointDown_ShopBtn, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_ShopSlot), OnPointUp_ShopBtn, UIEvent.PointUp);

        Sound_Buy = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Buy"], transform).GetComponent<AudioSource>();
        Sound_Sell = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Sell"], transform).GetComponent<AudioSource>();

        isInit = true;
    }
    public void Init(Data.EShopTabType type,int index)
    {
        Init();
        switch (type)
        {
            case Data.EShopTabType.BuyMaterial:
            case Data.EShopTabType.BuyAccessory:
                BindEvent(GetObject((int)Components.Button_ShopSlot), OnBtnClicked_BuyProduct, UIEvent.Click);
                GetObject((int)Components.Text_ShopSlotBtnName).GetComponent<TMP_Text>().text = "구매";
                break;
            case Data.EShopTabType.SellItem:
                BindEvent(GetObject((int)Components.Button_ShopSlot), OnBtnClicked_SellItem, UIEvent.Click);
                GetObject((int)Components.Text_ShopSlotBtnName).GetComponent<TMP_Text>().text = "판매";
                break;
        }

        itemID = Managers.DataLoder.DataCache_Shop[type].products[index].productid;
        price = Managers.DataLoder.DataCache_Shop[type].products[index].price;

        GetObject((int)Components.Text_PriceValue).GetComponent<TMP_Text>().text = $" X {price}";
        GetObject((int)Components.Image_ProductICon).GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[Managers.DataLoder.DataCache_Shop[type].products[index].iconpath];
    }
    public void OnBtnClicked_BuyProduct(PointerEventData data)
    {
        if (Managers.ContentsManager.ConsumeGold(price))
        {
            Sound_Buy.Play();
            Managers.ContentsManager.AddItem(itemID);
        }
        GetComponentInParent<ShopUI>().SetGold();
        GetComponentInParent<ShopUI>().TabClick(GetComponentInParent<ShopUI>().inventype);
    }
    public void OnBtnClicked_SellItem(PointerEventData data)
    {
        if (Managers.ContentsManager.RemoveItem(itemID))
        {
            Sound_Sell.Play();
            Managers.ContentsManager.AcquireGold(price); 
        }
        GetComponentInParent<ShopUI>().SetGold();
        GetComponentInParent<ShopUI>().TabClick(GetComponentInParent<ShopUI>().inventype);
    }
    
    public void OnPointDown_ShopBtn(PointerEventData data) 
    {
        GetObject((int)Components.Button_ShopSlot).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_ShopBtn(PointerEventData data) 
    {
        GetObject((int)Components.Button_ShopSlot).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }


}

