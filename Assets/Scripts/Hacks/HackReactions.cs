using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackReactions
{
    public static string[] Usernames = new string[] { 
        "霊夢ファン９", 
        "チルノ",
        "優しい魔女",
        "Gold_VAMPIRE777",
        "KappaPride",
        "R_月人631",
        "村人おじさん338_0XX",
        "にんぎょう使い",
        "名知らぬ妖怪",
        "安倍晋三",
        "_フランちゃんかわいい_",
        "ミスティア",
        "チルノの友達",
        "チルノの友達の友達",
        "アムロ・レイかんげき",
    };

    public static string[] NormalReactions = new string[] {
        "「こん霊夢!」",
        "「こんばんわ 初見です」",
        "「こん霊夢っっっっっっっ!!!!」",
        "「なにこれw」",
        "「草」",
        "「霊夢様今日も美しい」",
        "「え」",
    };
}

public class ReimuReactions
{
    public static ReimuAction[] GreetingReactions = new ReimuAction[] {
        new ReimuAction("こん霊夢!", 0, TweenExpressionType.Normal),
        new ReimuAction("博麗神社所属博麗霊夢です", 0, TweenExpressionType.Normal),
        new ReimuAction("今回はファンから頂いた自作ゲームをやろうと思います", 0, TweenExpressionType.Jump),
        new ReimuAction("耐久ゲームでらしいですね", 7, TweenExpressionType.Normal),
        new ReimuAction("そんな訳で、ゲームオーバーしたら即終了の配信になります", 6, TweenExpressionType.HappyTalk),
        new ReimuAction("初見のみんなもし良かったら", 0, TweenExpressionType.Normal),
        new ReimuAction("チャンネル登録と高評価お願いします", 7, TweenExpressionType.HappyTalk),
        new ReimuAction("それでは、始めます", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] ReturnLines = new ReimuAction[] {
        new ReimuAction("続きますか", 1, TweenExpressionType.Normal),
        new ReimuAction("もどりますね", 1, TweenExpressionType.Normal),
        new ReimuAction("。。。", 1, TweenExpressionType.Normal),
    };

    public static ReimuAction[] NormalReactions = new ReimuAction[] {
        new ReimuAction("よいしょ", 0, TweenExpressionType.Normal),
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("面白いですねこのゲーム", 0, TweenExpressionType.Normal),
        new ReimuAction("いい天気ですね", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] SpeedUPReactions = new ReimuAction[] {
        new ReimuAction("あれ？", 0, TweenExpressionType.Normal),
        new ReimuAction("なんか。。速くなってない", 0, TweenExpressionType.Normal),
    };
    public static ReimuAction[] AdWindowReactions = new ReimuAction[] {
        new ReimuAction("そうだ、発表あるんですよー", 0, TweenExpressionType.Jump),
        new ReimuAction("広告かよ", 0, TweenExpressionType.Normal),
        new ReimuAction("広告はいらない", 0, TweenExpressionType.Normal),
        new ReimuAction("うざいな", 0, TweenExpressionType.Jump),
        new ReimuAction("こんなにいいるの。。", 0, TweenExpressionType.Normal),
        new ReimuAction("消すわ", 0, TweenExpressionType.Normal),
    };
    public static ReimuAction[] HorrorReactions = new ReimuAction[] {
        new ReimuAction("わー！", 0, TweenExpressionType.Jump),
        new ReimuAction("なななななななにいまの", 0, TweenExpressionType.Normal),
        new ReimuAction("ホラーゲームなの？？？？", 0, TweenExpressionType.Normal),
    };
    public static ReimuAction[] R18Reactions = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("え", 3, TweenExpressionType.Normal),
        new ReimuAction("あ", 4, TweenExpressionType.Jump)
    };

    public static ReimuAction[] FlashReactions = new ReimuAction[] {
        new ReimuAction("あ", 0, TweenExpressionType.Jump),
        new ReimuAction("目があああああああああああ", 0, TweenExpressionType.SideStep),
        new ReimuAction("痛いいいいいい", 0, TweenExpressionType.HappyTalk),
    };
    public static ReimuAction[] SpriteSwapReactions = new ReimuAction[] {
        new ReimuAction("ん？", 0, TweenExpressionType.Normal),
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("私の顔がああああああああああ", 0, TweenExpressionType.Jump),
    };

    public static ReimuAction[] KaobareReactions = new ReimuAction[] {
        new ReimuAction("それでねー", 0, TweenExpressionType.Normal),
        new ReimuAction("あ、え？", 2, TweenExpressionType.Normal),
        new ReimuAction("あれ", 2, TweenExpressionType.Normal),
        new ReimuAction("なにこれ", 2, TweenExpressionType.Normal),
        new ReimuAction("俺の顔が。。", 2, TweenExpressionType.Normal),
        new ReimuAction("あ", 2, TweenExpressionType.Normal),
        new ReimuAction("ああああああああああああああああああああ", 2, TweenExpressionType.Jump),
        new ReimuAction("ちょ ちょっと待ってね", 2, TweenExpressionType.Normal),
        new ReimuAction("...", 0, TweenExpressionType.Jump),
        new ReimuAction("誰だろういまの。。", 0, TweenExpressionType.Normal),
        new ReimuAction("変な人ね", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] HPChangeReactionsGood = new ReimuAction[] {
        new ReimuAction("やったー", 0, TweenExpressionType.HappyTalk),
    };
    public static ReimuAction[] HPChangeReactionsBad = new ReimuAction[] {
        new ReimuAction("おおおいいいいいい", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] KnockKnockReactions = new ReimuAction[] {
        new ReimuAction("は", 0, TweenExpressionType.Normal),
        new ReimuAction("誰だろう", 1, TweenExpressionType.Normal),
        new ReimuAction("うるさいわ", 5, TweenExpressionType.Normal),
        new ReimuAction("聞いてないふりする", 0, TweenExpressionType.Normal),
    };
}
