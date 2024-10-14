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
        "八雲の隠れ家",
        "博麗の酒飲み",
        "守矢信者No1",
        "風見幽香推し",
        "空を飛びたい兎",
        "月の姫の影",
        "萃香の宴会部長",
        "幻の巫女代理",
        "笑顔の地蔵",
        "月夜のナマズ",
        "華仙の弟子",
        "山の賢者",
        "萃夢想ファン2024",
        "永夜の旅人",
        "氷精ファンクラブ"
    };

    public static string[] NormalReactions = new string[] {
        "「こん霊夢!」",
        "「こん霊夢!」",
        "「こんこん霊夢!」",
        "「こん霊夢」",
        "「こんれいむ」",
        "「こんばんわ 初見です」",
        "「こん霊夢っっっっっっっ!!!!」",
        "「なにこれw」",
        "「草」",
        "「霊夢様今日も美しい」",
        "「え」",
        "「草ｗ」",
        "「霊夢様かわいい」",
        "「笑」",
        "「wwwwwwwwwww」",
        "「霊夢ちゃん、がんばって！」",
        "「今日も平和ですね」",
        "「のんびりしてていい感じ」",
        "「霊夢様、今日はどうですか？」",
        "「お、いい感じ」",
        "「まったりと」",
        "「あれ？何か変わった？」",
        "「暇だなぁ」",
        "「今日の賽銭は？」",
        "「何も起こらないのが一番」",
        "「霊夢様大好き！」",
        "「霊夢様の声癒される」",
        "「まさかののんびり配信」",
        "「このまま何も起きないのがいいね」",
        "「霊夢、いつもありがとう」",
        "「またきたよ！」",
        "「誰かなんかするかな」"
    };
    public static string[] HackChatReaction = new string[] {
        "「何これ!?やばすぎw」",
        "「バグ！？画面がおかしい！」",
        "「え、何が起こってるの！？」",
        "「助けてｗｗｗｗｗ」",
        "「これ演出？本当に大丈夫？」",
        "「やばい、ハックされてる」",
        "「画面がチカチカしてる」",
        "「今の見た！？一瞬だったけど...」",
        "「うわっ、フラッシュバックが！？」",
        "「画面が乱れすぎて何も見えない...」",
        "「霊夢ちゃん、どうなっちゃうの...？」",
        "「混乱してるのは俺だけじゃないよね？」",
        "「なにこれ、ホラー？！」",
        "「カオスすぎるｗｗｗｗ」",
        "「霊夢様、大丈夫？顔が映ってたよ！」",
        "「完全にハックされてる！」",
        "「ちょっと、誰かこれ説明してｗｗｗ」",
        "「絶対に何かが起こってる...」",
        "「なにこれ、バグ？ハック？」",
        "「助けてｗｗｗ混乱してるｗｗｗ」",
        "「画面の乱れが止まらない...やばい」",
        "「霊夢ちゃん、負けないで！」"
    };
    public static string[] FlashbangReactions = new string[] {
        "「目が！目がああああ！」",
        "「うわ、眩しい！」",
        "「ちょっと、光が強すぎるよ！」",
        "「画面が真っ白なんだがｗｗｗ」",
        "「今の一瞬何も見えなかった！」",
        "「あれ？目がチカチカする...」",
        "「閃光弾か！？強すぎる！」",
        "「光りすぎて何も見えない」",
        "「うわ、まぶた閉じても光る！」",
        "「いきなりフラッシュやめてｗｗｗ」"
    };
    public static string[] FaceRevealReactions = new string[] {
        "「霊夢の顔！？！？！？」",
        "「顔が...霊夢様の顔が...」",
        "「うわ、霊夢の顔バレした！」",
        "「ちょ、今顔見えたよな？」",
        "「え！？霊夢の素顔が！！？」",
        "「顔が一瞬見えた...ドキドキ」",
        "「まさかの顔バレ！？」",
        "「霊夢様、美しすぎる...」",
        "「顔を出したのはわざと？」",
        "「霊夢ちゃん、まさか素顔が見れるとは」"
    };
    public static string[] MarisaIntrudingReactions = new string[] {
        "「あれ、マリサ！？？」",
        "「マリサが乱入してきたｗｗｗ」",
        "「え、マリサ何してんの？」",
        "「マリサ来ちゃったｗｗｗ」",
        "「マリサ乱入キターーーー！」",
        "「マリサのせいで何かおかしいぞ」",
        "「ちょ、マリサ！何やってんだｗ」",
        "「マリサ、突然の侵入やめてｗｗｗ」",
        "「なんでマリサがここに？」",
        "「マリサ乱入でカオスｗｗｗ」"
    };
    public static string[] HorrorJumpscareReactions = new string[] {
        "「ぎゃあああああ！」",
        "「びっくりした！」",
        "「怖すぎるってばｗｗｗ」",
        "「心臓止まるかと思った！」",
        "「今のはマジで怖かった」",
        "「霊夢様、ホラーはやめて！」",
        "「ちょ、心臓に悪い！」",
        "「怖すぎて椅子から落ちたｗｗｗ」",
        "「ぎゃあ！今の何！？」",
        "「もうホラー展開はやめて！」"
    };




    public static List<string[]> AkaSupaList = new List<string[]> {
        new string[] { "10人登録者数おめでとうございます霊夢様 それで告白したいことがあって、先日 はくれい神社の賽銭箱から百円盗んでました (笑)", "supa1", "3"},
        new string[] { "お祝いの言葉を！霊夢様、実は最近、神社の結界を少しだけいじくってしまいました…ごめんなさい！", "supa2", "5"},
        new string[] { "霊夢様、登録者数おめでとうございます！ところで、神社の鈴を壊しちゃったの、直してくれるかな？ごめんね〜！", "supa3", "4"},
        new string[] { "霊夢様、おめでとうございます！それで、あの霊夢様の漫画をこっそり借りちゃいました！返すの忘れたけど (笑)", "supa4", "6"},
    };

}

public class ReimuReactions
{
    public static ReimuAction[] GreetingReactions = new ReimuAction[] {
        new ReimuAction("あ、こん霊夢！", 0, TweenExpressionType.Normal, 1.0f),
        new ReimuAction("博麗神社の巫女、博麗霊夢です", 0, TweenExpressionType.Normal, 2.0f),
        new ReimuAction("今日はファンからもらった自作ゲームをやるわ", 0, TweenExpressionType.Jump, 1.5f),
        new ReimuAction("耐久ゲームらしいけど、まぁ大丈夫でしょう", 7, TweenExpressionType.Normal, 2.0f),
        new ReimuAction("だから、ゲームオーバーしたら即配信終了になるわ", 6, TweenExpressionType.HappyTalk, 2.5f),
        new ReimuAction("初見さんも、よかったら", 0, TweenExpressionType.Normal, 1.0f),
        new ReimuAction("チャンネル登録と高評価、よろしくね", 7, TweenExpressionType.HappyTalk, 1.5f),
        new ReimuAction("それじゃ、始めるわ", 0, TweenExpressionType.Normal, 1.0f),
    };


    public static ReimuAction[] SubCountMark1 = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal, 1.0f),
        new ReimuAction("あれ、ちょっと待って", 3, TweenExpressionType.Jump, 1.5f),
        new ReimuAction("えっ、登録者数が1000人いったの？", 3, TweenExpressionType.HappyTalk, 2.0f),
        new ReimuAction("ほんとに？", 3, TweenExpressionType.Normal, 1.5f),
        new ReimuAction("ま、まあみんなのおかげね", 7, TweenExpressionType.Normal, 2.0f),
        new ReimuAction("ありがと。これからもよろしく", 7, TweenExpressionType.HappyTalk, 2.0f),
    };
    public static ReimuAction[] SubCountMark100k = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal, 1.0f),
        new ReimuAction("えっ、今度は10万人！？", 7, TweenExpressionType.Jump, 1.5f),
        new ReimuAction("す、すごい。。。", 7, TweenExpressionType.HappyTalk, 2.0f),
        new ReimuAction("こんなに応援してくれるなんて", 7, TweenExpressionType.Normal, 1.5f),
        new ReimuAction("本当にみんなありがとう！", 7, TweenExpressionType.HappyTalk, 2.0f),
        new ReimuAction("これからも一緒に頑張ろうね！", 7, TweenExpressionType.HappyTalk, 2.5f),
    };
    public static ReimuAction[] SubCountMark1M = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal, 1.0f),
        new ReimuAction("ええ！？ 100万人！？？", 7, TweenExpressionType.SideStep, 1.5f),
        new ReimuAction("夢じゃないよね。。。", 7, TweenExpressionType.HappyTalk, 2.0f),
        new ReimuAction("みんな。。。本当にありがとう！！", 7, TweenExpressionType.HappyTalk, 2.5f),
        new ReimuAction("100万人なんて信じられない！", 7, TweenExpressionType.HappyTalk, 2.0f),
        new ReimuAction("これからもよろしくね！もっともっとがんばるから！", 7, TweenExpressionType.HappyTalk, 3.0f),
    };



    public static ReimuAction[] ReturnLines = new ReimuAction[] {
        new ReimuAction("はぁ、ようやく終わった...", 3, TweenExpressionType.Normal),  // Depressed (tired relief)
        new ReimuAction("じゃあ、もどるわね", 0, TweenExpressionType.Normal),  // Normal (calm return)
        new ReimuAction("。。。ふぅ", 1, TweenExpressionType.Normal),  // Blank (quiet reflection)
        new ReimuAction("ま、とにかく戻ります", 4, TweenExpressionType.Normal),  // Hiku (slightly awkward)
        new ReimuAction("これでやっと落ち着ける...", 0, TweenExpressionType.Normal),  // Normal (finally calming down)
        new ReimuAction("こんなの毎回やられたらたまんないわ", 5, TweenExpressionType.Normal),  // Raymoo (cursed face, irritated)
        new ReimuAction("あー、もう疲れた。。。", 3, TweenExpressionType.Normal),  // Depressed (exhausted)
        new ReimuAction("さ、気を取り直していこう", 7, TweenExpressionType.HappyTalk),  // Happy (optimistic return)
        new ReimuAction("ま、こんなもんでしょ", 0, TweenExpressionType.Normal),  // Normal (nonchalant, brushing it off)
        new ReimuAction("。。。次は静かに頼む", 4, TweenExpressionType.Normal),  // Hiku (slightly irritated, request for calm)
    };


    public static ReimuAction[] NormalReactions = new ReimuAction[] {
        new ReimuAction("よいしょ", 0, TweenExpressionType.Normal, 8f),  // Casual, lifting something
        new ReimuAction("。。。", 0, TweenExpressionType.Normal, 8f),  // Quiet, reflective
        new ReimuAction("おもしろいな", 0, TweenExpressionType.Normal, 8f),  // Finding amusement in something
        new ReimuAction("いいてんきですね", 0, TweenExpressionType.Normal, 8f),  // Casual, talking about the weather
        new ReimuAction("。。。", 0, TweenExpressionType.Normal, 8f),  // Another pause, thinking
        new ReimuAction("ん。。", 0, TweenExpressionType.Normal, 8f),  // Slight hesitation or thought
        new ReimuAction("なんか眠くなってきたわ", 0, TweenExpressionType.Normal, 8f),  // Feeling a bit sleepy
        new ReimuAction("ちょっと疲れたかも", 3, TweenExpressionType.Normal, 8f),  // A bit tired
        new ReimuAction("こんなの毎日やってられないわ", 4, TweenExpressionType.Normal, 8f),  // Slight frustration with monotony
        new ReimuAction("。。。何してんだっけ", 0, TweenExpressionType.Normal, 8f),  // Lost in thought, forgetful
    };


    public static ReimuAction[] SpeedUPReactions = new ReimuAction[] {
        new ReimuAction("あれ？", 0, TweenExpressionType.Normal),  // Slight confusion
        new ReimuAction("なんか。。速くなってない？", 0, TweenExpressionType.Normal),  // Noticing the speed change
        new ReimuAction("気のせいか", 0, TweenExpressionType.Normal),  // Shrugging it off
        new ReimuAction("これで、やるしかないわね", 4, TweenExpressionType.Normal),  // Determined, accepting the challenge
    };

    public static ReimuAction[] AdWindowReactions = new ReimuAction[] {
        new ReimuAction("広告かよ！", 5, TweenExpressionType.Normal),  // Frustrated with the ads, using her annoyed face
        new ReimuAction("何でこんなに出てくるの？", 3, TweenExpressionType.Normal),  // Exasperated, still maintaining a **depressed** expression
        new ReimuAction("うざい、消せ！", 4, TweenExpressionType.Jump),  // Annoyed and jumping in irritation
        new ReimuAction("もう無理！", 4, TweenExpressionType.Normal),  // Cursed face, at her breaking point
        new ReimuAction("なんでこんなことになるの？", 4, TweenExpressionType.Normal),  // Blank expression, confusion mixed with annoyance
        new ReimuAction("いい加減にしろ！", 0, TweenExpressionType.Normal),  // Back to normal, but firmly expressing her anger
    };
    public static ReimuAction[] HorrorReactions = new ReimuAction[] {
        new ReimuAction("わー！", 1, TweenExpressionType.Jump),  // Startled and jumping with anger
        new ReimuAction("な、なんでこんなのが出てくるの？", 5, TweenExpressionType.Jump),  // Confused and slightly irritated
        new ReimuAction("ホラーゲームなの？？？私に何を期待してるのよ！", 5, TweenExpressionType.Normal),  // Exasperated at the game, with a hint of fear
    };

    public static ReimuAction[] R18Reactions = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("え", 3, TweenExpressionType.Normal),
        new ReimuAction("えええええ", 5, TweenExpressionType.SideStep),
        new ReimuAction("まーまー まってー", 5, TweenExpressionType.Normal),
    };

    public static ReimuAction[] FlashReactions = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Jump),
        new ReimuAction("目があああああああああああ", 1, TweenExpressionType.SideStep),
        new ReimuAction("こんなのやめてよ！", 5, TweenExpressionType.HappyTalk),
        new ReimuAction("なんなんだよ、全く", 5, TweenExpressionType.Normal),
    };
    public static ReimuAction[] SpriteSwapReactions = new ReimuAction[] {
        new ReimuAction("ん？", 0, TweenExpressionType.Normal),
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("私の顔がああああああああああ", 0, TweenExpressionType.Jump),
    };



    public static ReimuAction[] PostKaobareReactions = new ReimuAction[] {
        new ReimuAction("おい！！！", 5, TweenExpressionType.Jump, 1f),
        new ReimuAction("またかよ！", 5, TweenExpressionType.Normal),
        new ReimuAction("。。。", 5, TweenExpressionType.Normal),
        new ReimuAction("配信機材の故障結構ありますね きょう", 1, TweenExpressionType.Normal, 3f),
    };
    public static ReimuAction[] KaobareReactions = new ReimuAction[] {
        new ReimuAction("それでねー", 0, TweenExpressionType.Normal),
        new ReimuAction("あー、え？", 2, TweenExpressionType.Normal),
        new ReimuAction("あれ", 2, TweenExpressionType.Normal),
        new ReimuAction("右下？", 2, TweenExpressionType.Normal),
        new ReimuAction("私の顔が。。", 2, TweenExpressionType.Normal),
        new ReimuAction("あ", 2, TweenExpressionType.Normal),
        new ReimuAction("ああああああああああああああああああああ", 2, TweenExpressionType.Jump),
        new ReimuAction("ちょ ちょっと待ってね", 2, TweenExpressionType.Normal),
        new ReimuAction("...", 2, TweenExpressionType.Jump),
        new ReimuAction("...", 0, TweenExpressionType.Jump),
        new ReimuAction("誰だろういまの。。", 0, TweenExpressionType.Normal),
        new ReimuAction("見なかったことにしてね", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] ShutdownReactions = new ReimuAction[] {
        new ReimuAction("なー！", 1, TweenExpressionType.Jump, 1f),
        new ReimuAction("は？？", 3, TweenExpressionType.Normal),
        new ReimuAction("なんでだよー", 5, TweenExpressionType.Normal, 1f),
    };

    public static ReimuAction[] HPChangeReactionsGood = new ReimuAction[] {
        new ReimuAction("あれ?、HPが増えた!", 1, TweenExpressionType.HappyTalk),
        new ReimuAction("やったー", 7, TweenExpressionType.HappyTalk),
        new ReimuAction("これでちょっと安心ね", 0, TweenExpressionType.HappyTalk),
        new ReimuAction("ふふ、まだまだいける！", 6, TweenExpressionType.HappyTalk),
    };
    public static ReimuAction[] HPChangeReactionsBad = new ReimuAction[] {
        new ReimuAction("おおおいいいいいい、なんでHPが減ってるの！？", 0, TweenExpressionType.Normal),
        new ReimuAction("こんなことで負けるわけにはいかない！", 3, TweenExpressionType.Normal),
        new ReimuAction("絶対許さないからな…", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] KnockKnockReactions = new ReimuAction[] {
        new ReimuAction("は", 0, TweenExpressionType.Jump),
        new ReimuAction("誰だろう", 1, TweenExpressionType.Normal),
        new ReimuAction("また誰かのいたずらか？無視しよう。", 5, TweenExpressionType.Normal),
        new ReimuAction("気にしないでおこうかな。", 0, TweenExpressionType.Normal),
        new ReimuAction("うるさくて集中できないわ…放っておこう。", 4, TweenExpressionType.Normal),
    };

    public static ReimuAction[] MarisaContactReactions = new ReimuAction[] {
        new ReimuAction("あれ？", 1, TweenExpressionType.Jump),
        new ReimuAction("まりさ？", 0, TweenExpressionType.Normal),
        new ReimuAction("どうしてこんな時間に", 0, TweenExpressionType.Normal),
        new ReimuAction("配信中って", 3, TweenExpressionType.Normal),
        new ReimuAction("。。。", 4, TweenExpressionType.Normal),
        new ReimuAction("。。。", 4, TweenExpressionType.Normal),
        new ReimuAction("勝手に入るな", 5, TweenExpressionType.Jump),
        new ReimuAction("。。。", 4, TweenExpressionType.Normal),
        new ReimuAction("無視するわ", 4, TweenExpressionType.Normal),
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
    };

    public static ReimuAction[] SupaReactions = new ReimuAction[] {
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("え？ あかすぱ？？？", 7, TweenExpressionType.Normal),
        new ReimuAction("収益化されてないのに", 1, TweenExpressionType.Normal),
        new ReimuAction("。。。", 0, TweenExpressionType.Normal),
        new ReimuAction("は？", 5, TweenExpressionType.Normal),
        new ReimuAction("おまえ ふざけるな 見つけ出すからな", 5, TweenExpressionType.Normal),
        new ReimuAction("本当に もう", 5, TweenExpressionType.Normal),
        new ReimuAction("。。。", 4, TweenExpressionType.Normal),
    };

    
}
