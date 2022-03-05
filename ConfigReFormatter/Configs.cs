using Newtonsoft.Json;
using System.Collections.Generic;

namespace ConfigReFormatter
{
    public class EntWatch
    {
        /*
                'entities' => [
                    'require' => [
                        'name', 'shortname', 'team', 'buttonclass', 'filtername', 'hasfiltername', 'hammerid', 'mode', 'maxamount'
                    ],
                    'optional' => [
                        'cooldown', 'maxuses', 'startcd', 'triggerid', 'containerid', 'isWall', 'level', 'children', 'holy', 'glow', 'hud', 'autotransfer'
                    ]
                ],
        */

        [JsonProperty("name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("shortname", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortName { get; set; }

        [JsonProperty("team", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int Team { get; set; }

        [JsonProperty("buttonclass", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string ButtonClass { get; set; }

        [JsonProperty("hasfiltername", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool HasFilter { get; set; }

        [JsonProperty("filtername", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string FilterName { get; set; }

        [JsonProperty("maxamount", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int MaxAmount { get; set; }

        [JsonProperty("mode", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int Mode { get; set; }

        [JsonProperty("hammerid", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int HammerId { get; set; }

        [JsonProperty("triggerid", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? TriggerId { get; set; }

        [JsonProperty("containerid", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? ContainerId { get; set; }

        [JsonProperty("maxuses", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxUses { get; set; }

        [JsonProperty("cooldown", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? Cooldown { get; set; }

        [JsonProperty("startcd", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? Startcd { get; set; }

        [JsonProperty("isWall", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? WallEntity { get; set; }

        [JsonProperty("glow", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? Glow { get; set; }

        [JsonProperty("hud", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? Hud { get; set; }

        [JsonProperty("autotransfer", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? AutoTransfer { get; set; }

        [JsonProperty("level", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Level { get; set; }

        [JsonProperty("children", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public EntChildren Children { get; set; }

        [JsonProperty("holy", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public EntHoly Holy { get; set; }
    }

    public class EntHoly
    {
        [JsonProperty("triggerid", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int TriggerId { get; set; }

        [JsonProperty("mode", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int Mode { get; set; }

        [JsonProperty("maxuses", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxUses { get; set; }

        [JsonProperty("cooldown", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? Cooldown { get; set; }

        [JsonProperty("class", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty("event", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }

        [JsonProperty("title", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public class EntChildren
    {
        [JsonProperty("type", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("case", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cases { get; set; }
    }

    public class BossHp
    {
        [JsonProperty("breakable", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, BossBreakable> Breakables { get; set; }

        [JsonProperty("counter", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, BossCounter> Counters { get; set; }

        [JsonProperty("monster", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, BossMonster> Monsters { get; set; }
    }

    public class BossBreakable
    {
        [JsonProperty("targetname", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string TargetName { get; set; }

        [JsonProperty("displayname", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("hpcounts", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }

        [JsonProperty("cashonly", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? CashOnly { get; set; }

        [JsonProperty("multiparts", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? MultiParts { get; set; }
    }

    public class BossCounter
    {
        [JsonProperty("iterator", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Iterator { get; set; }

        [JsonProperty("displayname", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("backup", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Backup { get; set; }

        [JsonProperty("counter", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Counter { get; set; }

        [JsonProperty("hitbox", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string HitBox { get; set; }

        [JsonProperty("countermode", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? HitMax { get; set; }

        [JsonProperty("multiparts", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? MultiParts { get; set; }
    }

    public class BossMonster
    {
        [JsonProperty("hammerid", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int HammerId { get; set; }

        [JsonProperty("displayname", DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }
    }

    public class Button
    {
        [JsonProperty("id", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int HammerId { get; set; }

        [JsonProperty("cd", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int Cooldown { get; set; }

        [JsonProperty("mode", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public int Mode { get; set; }

        [JsonProperty("name", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
