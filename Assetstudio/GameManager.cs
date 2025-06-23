using System;
using System.Linq;
using System.Collections.Generic;
using static AssetStudio.Crypto;

namespace AssetStudio
{
    public static class GameManager
    {
        private static Dictionary<int, Game> Games = new Dictionary<int, Game>();
        static GameManager()
        {
            int index = 0;
            Games.Add(index++, new(GameType.正常));
            Games.Add(index++, new(GameType.UnityCN));
            Games.Add(index++, new Mhy(GameTypeMapper.Map(GameType.GI), GIMhyShiftRow, GIMhyKey, GIMhyMul, GIExpansionKey, GISBox, GIInitVector, GIInitSeed));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.GI_Pack), PackExpansionKey, blockKey: PackBlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.GI_CB1)));
            Games.Add(index++, new Blk(GameTypeMapper.Map(GameType.GI_CB2), GI_CBXExpansionKey, initVector: GI_CBXInitVector, initSeed: GI_CBXInitSeed));
            Games.Add(index++, new Blk(GameTypeMapper.Map(GameType.GI_CB3), GI_CBXExpansionKey, initVector: GI_CBXInitVector, initSeed: GI_CBXInitSeed));
            Games.Add(index++, new Mhy(GameTypeMapper.Map(GameType.GI_CB3Pre), GI_CBXMhyShiftRow, GI_CBXMhyKey, GI_CBXMhyMul, GI_CBXExpansionKey, GI_CBXSBox, GI_CBXInitVector, GI_CBXInitSeed));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.BH3), BH3ExpansionKey, BH3SBox, BH3InitVector, BH3BlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.BH3Pre), PackExpansionKey, blockKey: PackBlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.BH3PrePre), PackExpansionKey, blockKey: PackBlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.SR_CB2), Mr0kExpansionKey, initVector: Mr0kInitVector, blockKey: Mr0kBlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.SR), Mr0kExpansionKey, initVector: Mr0kInitVector, blockKey: Mr0kBlockKey));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.ZZZ_CB1), Mr0kExpansionKey, initVector: Mr0kInitVector, blockKey: Mr0kBlockKey));
            Games.Add(index++, new Mhy(GameTypeMapper.Map(GameType.ZZZ_CB2), GIMhyShiftRow, GIMhyKey, GIMhyMul, null, GISBox, null, 0uL));
            Games.Add(index++, new Mhy(GameTypeMapper.Map(GameType.ZZZ), GIMhyShiftRow, GIMhyKey, GIMhyMul, null, GISBox, null, 0uL));
            Games.Add(index++, new Mr0k(GameTypeMapper.Map(GameType.TOT), Mr0kExpansionKey, initVector: Mr0kInitVector, blockKey: Mr0kBlockKey, postKey: ToTKey));
            Games.Add(index++, new Game(GameType.崩坏学园2));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.永劫无间)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.偶像梦幻祭2)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.航海王热血航线)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.FakeHeader)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.风之幻想)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.胜利女神妮姬)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.螺旋圆舞曲2蔷薇战争)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.NetEase)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.锚点降临)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.梦间集天鹅座)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.魔法禁书目录幻想收束)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.机甲爱丽丝)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.世界计划多彩舞台)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.jump群星集结)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.少女前线)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.重返未来1999)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.明日方舟_终末地)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.咒术回战幻影夜行)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.MuvLuv维度)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.动物派对)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.恋与深空)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.学园少女突袭者)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.来自星辰)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.物华弥新)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.无期迷途)));
            Games.Add(index++, new Game(GameTypeMapper.Map(GameType.望月)));
        }
        public static Game GetGame(GameType gameType) => GetGame((int)gameType);
        public static Game GetGame(int index)
        {
            if (!Games.TryGetValue(index, out var format))
            {
                throw new ArgumentException("无效的格式!!");
            }

            return format;
        }

        public static Game GetGame(string name) => Games.FirstOrDefault(x => x.Value.Name == name).Value;
        public static Game[] GetGames() => Games.Values.ToArray();
        public static string[] GetGameNames() => Games.Values.Select(x => x.Name).ToArray();
        public static string SupportedGames() => $"支持的游戏:\n{string.Join("\n", Games.Values.Select(x => x.Name))}";
    }

    public record Game
    {
        public string Name { get; set; }
        public GameType Type { get; }

        public Game(GameType type)
        {
            Name = type.ToString();
            Type = type;
        }

        public sealed override string ToString() => Name;
    }

    public record Mr0k : Game
    {
        public byte[] ExpansionKey { get; }
        public byte[] SBox { get; }
        public byte[] InitVector { get; }
        public byte[] BlockKey { get; }
        public byte[] PostKey { get; }

        public Mr0k(GameType type, byte[] expansionKey = null, byte[] sBox = null, byte[] initVector = null, byte[] blockKey = null, byte[] postKey = null) : base(type)
        {
            ExpansionKey = expansionKey ?? Array.Empty<byte>();
            SBox = sBox ?? Array.Empty<byte>();
            InitVector = initVector ?? Array.Empty<byte>();
            BlockKey = blockKey ?? Array.Empty<byte>();
            PostKey = postKey ?? Array.Empty<byte>();
        }
    }

    public record Blk : Game
    {
        public byte[] ExpansionKey { get; }
        public byte[] SBox { get; }
        public byte[] InitVector { get; }
        public ulong InitSeed { get; }

        public Blk(GameType type, byte[] expansionKey = null, byte[] sBox = null, byte[] initVector = null, ulong initSeed = 0) : base(type)
        {
            ExpansionKey = expansionKey ?? Array.Empty<byte>();
            SBox = sBox ?? Array.Empty<byte>();
            InitVector = initVector ?? Array.Empty<byte>();
            InitSeed = initSeed;
        }
    }

    public record Mhy : Blk
    {
        public byte[] MhyShiftRow { get; }
        public byte[] MhyKey { get; }
        public byte[] MhyMul { get; }

        public Mhy(GameType type, byte[] mhyShiftRow, byte[] mhyKey, byte[] mhyMul, byte[] expansionKey = null, byte[] sBox = null, byte[] initVector = null, ulong initSeed = 0) : base(type, expansionKey, sBox, initVector, initSeed)
        {
            MhyShiftRow = mhyShiftRow;
            MhyKey = mhyKey;
            MhyMul = mhyMul;
        }
    }

    public enum GameType
    {
        正常,
        UnityCN,
        GI,
        GI_Pack,
        GI_CB1,
        GI_CB2,
        GI_CB3,
        GI_CB3Pre,
        BH3,
        BH3Pre,
        BH3PrePre,
        ZZZ_CB1,
        ZZZ_CB2,
        ZZZ,
        SR_CB2,
        SR,
        TOT,
        崩坏学园2,
        永劫无间,
        偶像梦幻祭2,
        航海王热血航线,
        FakeHeader,
        风之幻想,
        胜利女神妮姬,
        螺旋圆舞曲2蔷薇战争,
        NetEase,
        锚点降临,
        梦间集天鹅座,
        魔法禁书目录幻想收束,
        机甲爱丽丝,
        世界计划多彩舞台,
        jump群星集结,
        少女前线,
        重返未来1999,
        明日方舟_终末地,
        咒术回战幻影夜行,
        MuvLuv维度,
        动物派对,
        恋与深空,
        学园少女突袭者,
        来自星辰,
        物华弥新,
        原神,
        崩坏三,
        崩坏星穹铁道,
        未定事件簿,
        无期迷途,
        望月,
    }

    public static class GameTypeMapper
    {
        private static readonly Dictionary<GameType, GameType> Mapping = new Dictionary<GameType, GameType>
        {
            { GameType.GI, GameType.原神 },
            { GameType.BH3, GameType.崩坏三 },
            { GameType.SR, GameType.崩坏星穹铁道 },
            { GameType.TOT, GameType.未定事件簿 }
        };

        public static GameType Map(GameType originalType)
        {
            if (Mapping.TryGetValue(originalType, out var mappedType))
            {
                return mappedType;
            }
            return originalType;
        }
    }

    public static class GameTypes
    {
        public static bool IsNormal(this GameType type) => type == GameTypeMapper.Map(GameType.正常);
        public static bool IsUnityCN(this GameType type) => type == GameTypeMapper.Map(GameType.UnityCN);
        public static bool IsGI(this GameType type) => type == GameTypeMapper.Map(GameType.GI);
        public static bool IsGIPack(this GameType type) => type == GameTypeMapper.Map(GameType.GI_Pack);
        public static bool IsGICB1(this GameType type) => type == GameTypeMapper.Map(GameType.GI_CB1);
        public static bool IsGICB2(this GameType type) => type == GameTypeMapper.Map(GameType.GI_CB2);
        public static bool IsGICB3(this GameType type) => type == GameTypeMapper.Map(GameType.GI_CB3);
        public static bool IsGICB3Pre(this GameType type) => type == GameTypeMapper.Map(GameType.GI_CB3Pre);
        public static bool IsBH3(this GameType type) => type == GameTypeMapper.Map(GameType.BH3);
        public static bool IsBH3Pre(this GameType type) => type == GameTypeMapper.Map(GameType.BH3Pre);
        public static bool IsBH3PrePre(this GameType type) => type == GameTypeMapper.Map(GameType.BH3PrePre);
        public static bool IsZZZCB1(this GameType type) => type == GameTypeMapper.Map(GameType.ZZZ_CB1);
        public static bool IsZZZCB2(this GameType type) => type == GameTypeMapper.Map(GameType.ZZZ_CB2);
        public static bool IsZZZ(this GameType type) => type == GameTypeMapper.Map(GameType.ZZZ);
        public static bool IsSRCB2(this GameType type) => type == GameTypeMapper.Map(GameType.SR_CB2);
        public static bool IsSR(this GameType type) => type == GameTypeMapper.Map(GameType.SR);
        public static bool IsTOT(this GameType type) => type == GameTypeMapper.Map(GameType.TOT);
        public static bool IsGGZ(this GameType type) => type == GameType.崩坏学园2;
        public static bool IsNaraka(this GameType type) => type == GameTypeMapper.Map(GameType.永劫无间);
        public static bool IsOPFP(this GameType type) => type == GameTypeMapper.Map(GameType.航海王热血航线);
        public static bool IsNetEase(this GameType type) => type == GameTypeMapper.Map(GameType.NetEase);
        public static bool IsArknightsEndfield(this GameType type) => type == GameTypeMapper.Map(GameType.明日方舟_终末地);
        public static bool IsWangYue(this GameType type) => type == GameType.望月;
        public static bool IsLoveAndDeepspace(this GameType type) => type == GameTypeMapper.Map(GameType.恋与深空);
        public static bool IsExAstris(this GameType type) => type == GameTypeMapper.Map(GameType.来自星辰);
        public static bool IsPerpetualNovelty(this GameType type) => type == GameTypeMapper.Map(GameType.物华弥新);
        public static bool Iswuqimitu(this GameType type) => type == GameTypeMapper.Map(GameType.无期迷途);
        public static bool IsGIGroup(this GameType type) => type switch
        {
            GameType.原神 or GameType.GI_Pack or GameType.GI_CB1 or GameType.GI_CB2 or GameType.GI_CB3 or GameType.GI_CB3Pre => true,
            _ => false,
        };

        public static bool IsGISubGroup(this GameType type) => type switch
        {
            GameType.原神 or GameType.GI_CB2 or GameType.GI_CB3 or GameType.GI_CB3Pre => true,
            _ => false,
        };

        public static bool IsBH3Group(this GameType type) => type switch
        {
            GameType.崩坏三 or GameType.BH3Pre => true,
            _ => false,
        };

        public static bool IsSRGroup(this GameType type) => type switch
        {
            GameType.崩坏星穹铁道 or GameType.SR_CB2 => true,
            _ => false,
        };

        public static bool IsBlockFile(this GameType type) => type switch
        {
            GameType.崩坏三 or GameType.BH3Pre or GameType.ZZZ_CB2 or GameType.ZZZ or GameType.崩坏星穹铁道 or GameType.GI_Pack or GameType.未定事件簿 or GameType.望月 or GameType.明日方舟_终末地 => true,
            _ => false,
        };

        public static bool IsMhyGroup(this GameType type) => type switch
        {
            GameType.原神 or GameType.GI_Pack or GameType.GI_CB1 or GameType.GI_CB2 or GameType.GI_CB3 or GameType.GI_CB3Pre or GameType.崩坏三 or GameType.BH3Pre or GameType.BH3PrePre or GameType.SR_CB2 or GameType.崩坏星穹铁道 or GameType.ZZZ_CB1 or GameType.ZZZ_CB2 or GameType.ZZZ or GameType.未定事件簿 => true,
            _ => false,
        };
    }
}
