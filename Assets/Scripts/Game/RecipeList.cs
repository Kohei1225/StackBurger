using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeList : MonoBehaviour
{
    #region typedef
    /// <summary> 商品の種類 </summary>
    public enum ProductType
    {
        BURGER,
        SANDWITCH,
        MUFFIN,
        DISH,
        PANCAKE,
        SALAD,
        BIG,
        RANDOM,
        TYPE_MAX,
    }

    /// <summary> 商品の情報をまとめた構造体 </summary>
    public struct ProductInfo
    {
        /// <summary> 商品の種類 </summary>
        public ProductType type;
        /// <summary> 商品名 </summary>
        public string name;
        /// <summary> 価格 </summary>
        public float value;
        /// <summary> レシピ </summary>
        public int[] recipe;
        /// <summary> 画像イメージ </summary>
        public Sprite image;
    };
    #endregion

    #region define
    /// <summary> 何もない画像 </summary>
    public static Sprite _Empty { get; } = Resources.Load<Sprite>("Sprites/Empty");

    /// <summary> ハンバーガー(最安) - 100円 </summary>
    private static ProductInfo _Burger_HumburgerCheap = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "格安バーガー",
        value = 1.20f,
        recipe = new int[] { 1, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Burger/Hamburger(poor)"),
    };
    /// <summary> ハンバーガー(安い) - 100円 </summary>
    private static ProductInfo _Burger_HumburgerLow = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "安いバーガー",
        value = 2.20f,
        recipe = new int[] { 1, 4, 19, 5, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Burger/Hamburger(low)"),
    };
    /// <summary> ハンバーガー(普通) - 150円 </summary>
    private static ProductInfo _Burger_Humburger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ハンバーガー",
        value = 2.80f,
        recipe = new int[] { 1, 4, 19, 5, 6, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Burger/Hamburger"),
    };
    /// <summary> ハンバーガー(高い) - 180円 </summary>
    private static ProductInfo _Burger_HumburgerHigh = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ナイスバーガー",
        value = 3.20f,
        recipe = new int[] { 1, 4, 19, 5, 6, 7, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Burger/Hamburger(good)"),
    };
    /// <summary> ハンバーガー(最高) - 100円 </summary>
    private static ProductInfo _Burger_HumburgerRich = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "リッチバーガー",
        value = 4.00f,
        recipe = new int[] { 1, 13, 19, 5, 6, 7, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Burger/Hamburger(best)"),
    };

    /// <summary> チーズバーガー(最安) - 100円 </summary>
    private static ProductInfo _Burger_CheeseCheap = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "格安\nチーズバーガー",
        value = 1.60f,
        recipe = new int[] { 1, 4, 9, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Cheese(poor)"),
    };
    /// <summary> チーズバーガー(安い) - 200円 </summary>
    private static ProductInfo _Burger_CheeseLow = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "安い\nチーズバーガー",
        value = 2.60f,
        recipe = new int[] { 1, 4, 9, 19, 5, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Cheese(low)"),
    };
    /// <summary> チーズバーガー(普通) - 250円 </summary>
    private static ProductInfo _Burger_Cheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "チーズバーガー",
        value = 3.30f,
        recipe = new int[] { 1, 4, 9, 19, 5, 6, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Cheese"),
    };

    /// <summary> チーズバーガー(高い) - 250円 </summary>
    private static ProductInfo _Burger_CheeseHigh = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ナイス\nチーズバーガー",
        value = 3.80f,
        recipe = new int[] { 1, 4, 9, 19, 5, 6, 7, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Cheese(nice)"),
    };
    /// <summary> チーズバーガー(最高) - 250円 </summary>
    private static ProductInfo _Burger_CheeseRich = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "リッチ\nチーズバーガー",
        value = 4.50f,
        recipe = new int[] { 1, 13, 9, 19, 5, 6, 7, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Cheese(best)"),
    };
    /// <summary> ベーコンチーズバーガー </summary>
    private static ProductInfo _Burger_BaconCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ベーコン\nチーズバーガー",
        value = 5.00f,
        recipe = new int[] { 1, 20, 4, 9, 14, 8, 19, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/BaconCheese"),
    };
    /// <summary> ダブルチーズバーガー </summary>
    private static ProductInfo _Burger_DoubleCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ダブル\nチーズバーガー",
        value = 4.20f,
        recipe = new int[] { 1, 4, 9, 4, 9, 19, 5, 6, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Double"),
    };
    /// <summary> トリプルチーズバーガー </summary>
    private static ProductInfo _Burger_TripleCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "トリプル\nチーズバーガー",
        value = 5.40f,
        recipe = new int[] { 1, 4, 9, 4, 9, 4, 9, 19, 5, 6, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Cheese/Triple"),
    };

    /// <summary> カツバーガー(安い) </summary>
    private static ProductInfo _Burger_KatsuCheap = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "安い\nカツバーガー",
        value = 2.3f,
        recipe = new int[] { 1, 10, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Katsu/Katsu(poor)"),
    };
    /// <summary> カツバーガー(普通) - 250円 </summary>
    private static ProductInfo _Burger_KatsuBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "カツバーガー",
        value = 2.80f,
        recipe = new int[] { 1, 10, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Katsu/Katsu"),
    };
    /// <summary> フィッシュバーガー(普通) - 250円 </summary>
    private static ProductInfo _BurgerFishBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "フィッシュ\nバーガー",
        value = 2.40f,
        recipe = new int[] { 1, 9, 10, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Katsu/Fish"),
    };

    /// <summary> てりやきバーガー(安い) - 250円 </summary>
    private static ProductInfo _Burger_TeriyakiCheap = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "安い\nてりやき\nバーガー",
        value = 1.2f,
        recipe = new int[] { 1, 4, 18, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teriyaki/Teriyaki(poor)"),
    };
    /// <summary> てりやきバーガー(普通) - 250円 </summary>
    private static ProductInfo _Burger_TeriyakiBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "てりやき\nバーガー",
        value = 3.2f,
        recipe = new int[] { 1, 4, 18, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teriyaki/Teriyaki"),
    };
    /// <summary> てりたまバーガー </summary>
    private static ProductInfo _Burger_TeritamaBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "たまてり\nバーガー",
        value = 4.2f,
        recipe = new int[] { 1, 4, 18, 11, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teritama/Teritama"),
    };
    /// <summary> チーズてりたまバーガー </summary>
    private static ProductInfo _Burger_TeritamaCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "チーズ\nたまてり\nバーガー",
        value = 4.8f,
        recipe = new int[] { 1, 4, 18, 11, 9, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teritama/CheeseTeritama"),
    };
    /// <summary> とんかつてりたまバーガー </summary>
    private static ProductInfo _Burger_TeritamaKatsu = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "とんかつ\nたまてり\nバーガー",
        value = 5.20f,
        recipe = new int []{ 1, 10, 18, 11, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teritama/KatsuTeritama"),
    };
    /// <summary> ベーコンてりたまバーガー </summary>
    private static ProductInfo _Burger_TeritamaBacon = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ベーコン\nたまてり\nバーガー",
        value = 4.2f,
        recipe = new int[] { 1, 4, 18, 11, 14, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Teritama/BaconTeritama"),
    };

    /// <summary> スタックバーガー </summary>
    private static ProductInfo _Burger_StackBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "スタック\nバーガー",
        value = 5.00f,
        recipe = new int[] { 1, 13, 19, 5, 8, 7, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Stack/Stack"),
    };
    /// <summary> スタックチーズバーガー </summary>
    private static ProductInfo _Burger_StackCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "スタック\nチーズバーガー",
        value = 6.0f,
        recipe = new int[] { 1, 13, 19, 5, 9, 8, 7, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Stack/StackCheese"),
    };

    /// <summary> お月見バーガー </summary>
    private static ProductInfo _Burger_OtsukimiBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "お月見バーガー",
        value = 4.2f,
        recipe = new int[] { 1, 4, 11, 14, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Otsukimi/Otsukimi"),
    };
    /// <summary> チーズお月見バーガー </summary>
    private static ProductInfo _Burger_OtsukimiCheese = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "チーズ\nお月見バーガー",
        value = 4.8f,
        recipe = new int[] { 1, 4, 9, 11, 14, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Otsukimi/CheeseOtsukimi"),
    };

    /// <summary> ビッグバーガー </summary>
    private static ProductInfo _Burger_BigBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ビッグバーガー",
        value = 6.20f,
        recipe = new int[] { 1, 20, 6, 9, 4, 2, 20, 6, 5, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Big/Big"),
    };

    /// <summary> メガバーガー </summary>
    private static ProductInfo _Burger_MegaBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "メガバーガー",
        value = 7.20f,
        recipe = new int[] { 1, 20, 6, 9, 4, 4, 2, 20, 6, 5, 4, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Big/Mega"),
    };
    /// <summary> ペタバーガー </summary>
    private static ProductInfo _Burger_PetaBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ペタバーガー",
        value = 8.20f,
        recipe = new int[] { 1, 20, 6, 9, 4, 4, 4, 2, 20, 6, 5, 4, 4, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Big/Peta"),
    };
    /// <summary> ヨタバーガー </summary>
    private static ProductInfo _Burger_YottaBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ヨタバーガー",
        value = 9.20f,
        recipe = new int[] { 1, 20, 6, 9, 4, 4, 4, 4, 2, 20, 6, 5, 4, 4, 4, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Big/Yotta"),
    };
    /// <summary> 全部盛りバーガー </summary>
    private static ProductInfo _Burger_AllBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "全部乗せ\nバーガー",
        value = 10.00f,
        recipe = new int[] { 1, 4, 19, 5, 9, 2, 10, 8, 6, 20, 14, 2, 13, 7, 12, 11, 18, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Big/All"),
    };

    /// <summary> アボカドバーガー </summary>
    private static ProductInfo _Burger_AbocadoBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "アボカド\nバーガー",
        value = 6.4f,
        recipe = new int[] { 1, 4, 5, 12, 19, 8, 7, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Else/Abocado"),
    };
    /// <summary> ベーコンレタスバーガー </summary>
    private static ProductInfo _Burger_BaconLettuceBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ベーコンレタス\nバーガー",
        value = 4.80f,
        recipe = new int[] { 1, 4, 14, 9, 6, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Else/BaconLettuce"),
    };
    /// <summary> 野菜バーガー </summary>
    private static ProductInfo _Burger_VegetableBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "野菜バーガー",
        value = 5.20f,
        recipe = new int[] { 1, 5, 8, 7, 6, 20, 12, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Else/Vegetable"),
    };
    /// <summary> フレッシュバーガー </summary>
    private static ProductInfo _Burger_FreshBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "フレッシュ\nバーガー",
        value = 6.00f,
        recipe = new int[] { 1, 13, 18, 11, 7, 8, 12, 6, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Else/Fresh"),
    };
    /// <summary> 肉増しバーガー </summary>
    private static ProductInfo _Burger_MeatBurger = new ProductInfo
    {
        type = ProductType.BURGER,
        name = "ミートバーガー",
        value = 5.50f,
        recipe = new int[] { 1, 4, 6, 20, 13, 14, 19, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Burger/Else/Meat"),
    };

    /// <summary> カツサンド </summary>
    private static ProductInfo _Sand_KatsuSand = new ProductInfo
    {
        type = ProductType.SANDWITCH,
        name = "カツサンド",
        value = 3.20f,
        recipe = new int[] { 2, 10, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Sandwich/KatsuSand"),
    };
    /// <summary> 卵サンド </summary>
    private static ProductInfo _Sand_EggSand = new ProductInfo
    {
        type = ProductType.SANDWITCH,
        name = "たまごサンド",
        value = 3.50f,
        recipe = new int[] { 2, 11, 6, 20, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Sandwich/EggSand"),
    };
    /// <summary> 野菜サンド </summary>
    private static ProductInfo _Sand_VegetableSand = new ProductInfo
    {
        type = ProductType.SANDWITCH,
        name = "野菜サンド",
        value = 4.00f,
        recipe = new int[] { 2, 7, 6, 20, 8, 12, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Sandwich/VegetableSand"),
    };
    /// <summary> ミートサンド </summary>
    private static ProductInfo _Sand_MeatSand = new ProductInfo
    {
        type = ProductType.SANDWITCH,
        name = "ミートサンド",
        value = 3.70f,
        recipe = new int[] { 2, 4, 18, 6, 13, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Sandwich/MeatSand"),
    };
    /// <summary> ベーコンチーズサンド </summary>
    private static ProductInfo _Sand_BaconSand = new ProductInfo
    {
        type = ProductType.SANDWITCH,
        name = "ベーコンサンド",
        value = 3.5f,
        recipe = new int[] { 2, 9, 14, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Sandwich/BaconCheeseSand"),
    };
    /// <summary> ソーセージマフィン </summary>
    private static ProductInfo _Muffin_SousageMuffine = new ProductInfo
    {
        type = ProductType.MUFFIN,
        name = "ソーセージ\nマフィン",
        value = 3.80f,
        recipe = new int[] { 15, 4, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Muffin/SousageMuffin"),
    };
    /// <summary> メガマフィン </summary>
    private static ProductInfo _Muffin_MegaMuffine = new ProductInfo
    {
        type = ProductType.MUFFIN,
        name = "メガマフィン",
        value = 4.50f,
        recipe = new int[] { 15, 4, 9, 4, 11, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Muffin/MegaMuffin"),
    };
    /// <summary> ベーコンエッグマフィン </summary>
    private static ProductInfo _Muffin_BaconEggMuffine = new ProductInfo
    {
        type = ProductType.MUFFIN,
        name = "ベーコンエッグ\nマフィン",
        value = 4.00f,
        recipe = new int[] { 15, 9, 14, 11, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Muffin/BaconEggMuffin"),
    };
    /// <summary> ソーセージエッグマフィン </summary>
    private static ProductInfo _Muffin_SousageEggMuffine = new ProductInfo
    {
        type = ProductType.MUFFIN,
        name = "ソーセージ\nエッグマフィン",
        value = 4.00f,
        recipe = new int[] { 15, 9, 4, 11, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Muffin/SousageEggMuffin"),
    };
    /// <summary> てりたまマフィン </summary>
    private static ProductInfo _Muffin_TeritamaMuffine = new ProductInfo
    {
        type = ProductType.MUFFIN,
        name = "てりたま\nマフィン",
        value = 6.00f,
        recipe = new int[] { 15, 4, 18, 11, 6, 20, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Muffin/TeritamaMuffin"),
    };
    /// <summary> 目玉焼き </summary>
    private static ProductInfo _Dish_Egg = new ProductInfo
    {
        type = ProductType.DISH,
        name = "目玉焼き",
        value = 1.2f,
        recipe = new int[] { 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Egg"),
    };
    /// <summary> ベーコンエッグ </summary>
    private static ProductInfo _Dish_BaconEgg = new ProductInfo
    {
        type = ProductType.DISH,
        name = "ベーコンエッグ",
        value = 2.2f,
        recipe = new int[] { 14, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/BaconEgg"),
    };
    /// <summary> ハンバーグ </summary>
    private static ProductInfo _Dish_Steak = new ProductInfo
    {
        type = ProductType.DISH,
        name = "ハンバーグ",
        value = 3.3f,
        recipe = new int[] { 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Steak"),
    };
    /// <summary> チーズハンバーグ </summary>
    private static ProductInfo _Dish_SteakCheese = new ProductInfo
    {
        type = ProductType.DISH,
        name = "チーズ\nハンバーグ",
        value = 3.8f,
        recipe = new int[] { 13, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/CheeseSteak"),
    };
    /// <summary> 目玉焼きハンバーグ </summary>
    private static ProductInfo _Dish_SteakEgg = new ProductInfo
    {
        type = ProductType.DISH,
        name = "目玉ハンバーグ",
        value = 3.8f,
        recipe = new int[] { 13, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/EggSteak"),
    };
    /// <summary> 目玉チーズハンバーグ </summary>
    private static ProductInfo _Dish_SteakEggCheese = new ProductInfo
    {
        type = ProductType.DISH,
        name = "目玉\nチーズ\nハンバーグ",
        value = 4.0f,
        recipe = new int[] { 13, 9, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/EggCheeseSteak"),
    };
    /// <summary> カツ </summary>
    private static ProductInfo _Dish_Katsu = new ProductInfo
    {
        type = ProductType.DISH,
        name = "カツ",
        value = 3.0f,
        recipe = new int[] { 6, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Katsu"),
    };
    /// <summary> パンケーキ1 </summary>
    private static ProductInfo _Dish_Pancake1 = new ProductInfo
    {
        type = ProductType.PANCAKE,
        name = "パンケーキ\n(1段)",
        value = 3.2f,
        recipe = new int[] { 15, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Pancake1"),
    };
    /// <summary> パンケーキ2 </summary>
    private static ProductInfo _Dish_Pancake2 = new ProductInfo
    {
        type = ProductType.PANCAKE,
        name = "パンケーキ\n(2段)",
        value = 4.2f,
        recipe = new int[] { 15, 16, 15, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Pancake2"),
    };
    /// <summary> パンケーキ3 </summary>
    private static ProductInfo _Dish_Pancake3 = new ProductInfo
    {
        type = ProductType.PANCAKE,
        name = "パンケーキ\n(3段)",
        value = 5.2f,
        recipe = new int[] { 15, 16, 15, 16, 15, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Pancake3"),
    };
    /// <summary> パンケーキ4 </summary>
    private static ProductInfo _Dish_Pancake4 = new ProductInfo
    {
        type = ProductType.PANCAKE,
        name = "パンケーキ\n(4段)",
        value = 6.2f,
        recipe = new int[] { 15, 16, 15, 16, 15, 16, 15, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Pancake4"),
    };
    /// <summary> パンケーキ5 </summary>
    private static ProductInfo _Dish_Pancake5 = new ProductInfo
    {
        type = ProductType.PANCAKE,
        name = "パンケーキ\n(5段)",
        value = 7.2f,
        recipe = new int[]{ 15, 16, 15, 16, 15, 16, 15, 16, 15, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Pancake5"),
    };
    /// <summary> 59.サラダ </summary>
    private static ProductInfo _Dish_Salada = new ProductInfo
    {
        type = ProductType.SALAD,
        name = "サラダ",
        value = 3.40f,
        recipe = new int[] { 5, 8, 7, 12, 6, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Dish/Salada"),
    };
    /// <summary> おまかせ </summary>
    private static ProductInfo _Random = new ProductInfo
    {
        type = ProductType.RANDOM,
        name = "おまかせ\n(なんでもいい)",
        value = 4.50f,
        recipe = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        image = Resources.Load<Sprite>("Sprites/Menu/Random"),
    };
    #endregion

    /// <summary> 商品のメニュー </summary>
    private static ProductInfo[] _Menu = new ProductInfo[]
    {
        //初日・２日目 (簡単なやつら)
        _Burger_Humburger,
        _Burger_HumburgerCheap,
        _Burger_HumburgerLow,
        _Burger_HumburgerHigh,
        //---------4----------

        //３日目 (バーガー系)
        _Burger_HumburgerRich,
        _Burger_CheeseCheap,
        _Burger_Cheese,
        _Burger_CheeseLow,
        _Burger_CheeseHigh,
        _Burger_CheeseRich,
        _Burger_BaconCheese,
        _Burger_TeriyakiCheap,
        _Burger_TeriyakiBurger,
        _Burger_TeritamaBurger,
        _Burger_TeritamaBacon,
        _Burger_TeritamaCheese,
        _Burger_TeritamaKatsu,
        _BurgerFishBurger,
        _Burger_KatsuCheap,
        _Burger_KatsuBurger,
        _Burger_StackBurger,
        _Burger_StackCheese,
        _Burger_AbocadoBurger,
        _Burger_BaconLettuceBurger,
        _Burger_VegetableBurger,
        _Burger_FreshBurger,
        _Burger_MeatBurger,
        _Burger_OtsukimiBurger,
        _Burger_OtsukimiCheese,
        //---------29----------

        //４日目 (サンドイッチ)
        _Sand_EggSand,
        _Sand_MeatSand,
        _Sand_VegetableSand,
        _Sand_BaconSand,
        _Sand_KatsuSand,
        //---------34----------

        //５日目 (マフィン)
        _Muffin_SousageMuffine,
        _Muffin_SousageEggMuffine,
        _Muffin_MegaMuffine,
        _Muffin_TeritamaMuffine,
        _Muffin_BaconEggMuffine,
        //---------39----------

        //６日目 (デカいバーガー)
        _Random,
        _Burger_DoubleCheese,
        _Burger_TripleCheese,
        _Burger_BigBurger,
        _Burger_MegaBurger,
        _Burger_PetaBurger,
        _Burger_YottaBurger,
        _Burger_AllBurger,
        //---------46----------

        //７日目 (料理)
        _Dish_Egg,
        _Dish_BaconEgg,
        _Dish_Steak,
        _Dish_SteakCheese,
        _Dish_SteakEgg,
        _Dish_SteakEggCheese,
        _Dish_Katsu,
        _Dish_Pancake1,
        _Dish_Pancake2,
        _Dish_Pancake3,
        _Dish_Pancake4,
        _Dish_Pancake5,
        _Dish_Salada,
        //---------59----------
    };

    #region property
    /// <summary> メニュー </summary>
    public static ProductInfo[] Menu
    {
        get { return _Menu; }
    }
    #endregion

    #region public function
    public static int CalcHeight(int[] array)
    {
        int i;
        for(i = 0;i < array.Length;i++)
        {
            if(array[i] == 0)
            {
                return i;
            }
        }
        return i;
    }
    #endregion
}
