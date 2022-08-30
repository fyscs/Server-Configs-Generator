using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kxnrl.FyS.ConfigConverter
{
    public class Command
    {
        public string Cmd;
        public string[] Arg;

        public Command(string cmd, params string[] arg)
        {
            Cmd = cmd;
            Arg = arg;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(256);
            sb.Append(Cmd);
            foreach (var arg in Arg)
            {
                sb.Append(" ");
                sb.Append(arg);
            }
            return sb.ToString();
        }
    }

    public class Variable
    {
        public string ConVar;
        public string DefVal;
        public string Comment;
        public string Group;
        public string Min;
        public string Max;

        public Variable(string cvar, string val, string group, string comment, string min = null, string max = null)
        {
            ConVar = cvar;
            DefVal = val;
            Group = group;
            Comment = comment;
            Min = min;
            Max = max;
        }

        public Variable Clone()
        {
            return new Variable(ConVar, DefVal, Group, Comment, Min, Max);
        }

        public override string ToString() => (string.IsNullOrEmpty(Min) && string.IsNullOrEmpty(Max))
            ? $"// 说  明: {Comment}{Environment.NewLine}{ConVar} \"{DefVal}\"{Environment.NewLine}"
            : $"// 说  明: {Comment}{Environment.NewLine}// 最小值: {Min}{Environment.NewLine}// 最大值: {Max}{Environment.NewLine}{ConVar} \"{DefVal}\"{Environment.NewLine}";
    }

    static class Program
    {
        private const string Versioning = "v3";

        private static readonly string MySelf = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static readonly string Worker = Path.Combine(MySelf, "worker");
        private static readonly string Output = Path.Combine(MySelf, "output");
        private static readonly Dictionary<string, List<Variable>> Configs = new Dictionary<string, List<Variable>>();

        static void Main()
        {
            Console.Title = "Configs Converter by Kyle";

            PrepareConfigs();

            Directory.CreateDirectory(Worker);
            Directory.CreateDirectory(Output);

            var count = 0;
            var succs = 0;

            Directory.GetFiles(Worker, "*.cfg", SearchOption.TopDirectoryOnly).ToList().ForEach(path =>
            {
                count++;

                var file = Path.GetFileName(path);
                try
                {
                    ParseConfig(path);
                    succs++;
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Parse [{file}] -> {e.Message}{Environment.NewLine}{e.StackTrace}");
                }
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Processed {succs} / {count} files.");
            Console.ReadKey(true);
        }

        private static void ParseConfig(string filePath)
        {
            var map = Path.GetFileNameWithoutExtension(filePath);
            if (string.IsNullOrEmpty(map))
            {
                // ?
                throw new Exception($"Failed to get map name in {filePath}");
            }

            if (!Configs.TryGetValue(map.Substring(0, 2), out var configs))
            {
                // no-support
                throw new Exception($"Map type {map.Substring(0, 2)}");
            }

            map = map.ToLower();

            var mapConf = new Dictionary<string, string>();
            var mapCmds = new List<Command>();

            File.ReadAllLines(filePath, Encoding.UTF8).ToList().ForEach(x =>
            {
                var line = x.Trim();
                if (line.StartsWith("//") || line.Length < 3 || line.ToLower().StartsWith("echo"))
                {
                    // comment line || blank line
                    return;
                }

                var split = line.IndexOf(' ');
                if (split == -1)
                {
                    // not found
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"Failed to parse line {line}");
                    return;
                }

                // conVar or command
                var cvar = line.Substring(0, split);
                var cval = line.Substring(split + 1);

                var quotes = line.Count(c => c == '"');

                // it's command
                if (quotes > 2 && quotes % 2 == 0)
                {
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"Found command [{cvar}] => {cval}");
                    mapCmds.Add(new Command(cvar, cval.Split(new char[] { ' ' })));
                    return;
                }
                else if (configs.FirstOrDefault(c => c.ConVar == cvar) == null)
                {
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"Invalid convar [{cvar}] => {cval}");
                    return;
                }

                cval = cval.Replace("\"", "").Trim();

                if (mapConf.ContainsKey(cvar))
                {
                    // already in
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"Skip duplicate convar [{cvar}]");
                    return;
                }

                mapConf.Add(cvar, cval);
            });

            var lines = new List<string>
            {
                $"// This file was auto-generated by ZombiEscape - Map Configs (https://github.com/fys-csgo/Server-Configs-Generator)",
                $"// ConVars and Commands for {map}",
                $"// Config Version {Versioning}",
                $"// ",
                $"// Kyle 'Kxnrl' Frankiss",
                $"// https://www.kxnrl.com",
                $"",
                $"",
                $""
            };

            lines.Add("/////////////////////");
            lines.Add("///    ConVars    ///");
            lines.Add("/////////////////////");
            var group = "";
            configs.ForEach(conf =>
            {
                var convar = conf.Clone();
                var config = mapConf.FirstOrDefault(c => c.Key == conf.ConVar);
                if (config.Key != null)
                {
                    convar.DefVal = config.Value;
                }

                if (convar.Group != group)
                {
                    lines.Add($"");
                    lines.Add($"///");
                    lines.Add($"/// {convar.Group}");
                    lines.Add($"///");
                    lines.Add($"");
                    group = convar.Group;
                }

                lines.Add(convar.ToString());
            });

            if (mapCmds.Count > 0)
            {
                lines.Add($"");
                lines.Add("//////////////////////");
                lines.Add("///    Commands    ///");
                lines.Add("//////////////////////");
                lines.Add($"");
                mapCmds.ForEach(cmd => { lines.Add(cmd.ToString()); });
                lines.Add($"");
            }

            lines.Add($"");
            lines.Add($"Echo \"Executed config for {map}.\"");

            File.WriteAllLines(Path.Combine(Output, map + "." + "cfg"), lines.ToArray(), new UTF8Encoding(false));
        }

        #region PrepareConfigs

        static void PrepareConfigs()
        {
            Configs["ze"] = new List<Variable>
            {
                // Global
                new Variable("mp_timelimit", "15", "Global", "地图时间 (分钟)", "1", "60"),
                new Variable("mp_roundtime", "30", "Global", "回合时间 (分钟)", "1", "60"),

                // Vote
                new Variable("vips_map_extend_times", "2", "Vote", "VIP延长投票 (次)", "0", "2"),
                new Variable("mcr_map_extend_times", "2", "Vote", "MCR延长投票 (次)", "0", "2"),

                // Misc
                //new Variable("sv_falldamage_scale", "0", "Misc", "坠落伤害 (%)", "0.0", "3.0"),
                new Variable("store_thirdperson_enabled", "1", "Misc", "第三人称视角控制 (开关)", "0", "1"),

                // Core
                new Variable("ze_infection_sort_by_keephuman_desc", "1", "Core", "根据尸变指数降序来选择母体僵尸<关闭则使用纯随机> (开关)", "0", "1"),
                new Variable("zr_infect_mzombie_ratio", "7", "Core", "尸变比 (人)", "1", "32"),
                new Variable("zr_infect_mzombie_respawn", "1", "Core", "尸变时传送回出生点 (开关)", "0", "1"),
                new Variable("zr_infect_spawntime_max", "15", "Core", "尸变倒计时<须与zr_infect_spawntime_min同步> (秒)", "1", "90"),
                new Variable("zr_infect_spawntime_min", "15", "Core", "尸变倒计时<须与zr_infect_spawntime_max同步> (秒)", "1", "90"),
                //new Variable("zr_respawn_delay", "5", "Core", "死亡复活延迟 (秒)", "1", "10"),
                new Variable("zr_knockback_multi", "1.0", "Core", "全局击退系数 (%)", "0.1", "1.5"),
                //new Variable("zr_classes_csgo_knockback_airmultiplier", "0.6", "Core", "空中击退系数 (%)", "0.1", "1.5"),
                //new Variable("zr_ztele_max_human", "1", "Core", "人类传送 (次)", "0", "3"),
                //new Variable("zr_ztele_max_zombie", "1", "Core", "僵尸传送 (次)", "0", "3"),

                // Rank
                new Variable("ze_damage_zombie_cash", "0.4", "Rank", "伤害与金钱转化比例 ($)", "0.1", "3.0"),
                new Variable("ze_damage_rank_points", "30000", "Rank", "伤害云点转化比例 (云点)", "5000.0", "99999.0"),
                new Variable("ze_damage_shop_credit", "50000", "Rank", "伤害积分转化比例 (积分)", "5000.0", "99999.0"),
                new Variable("ze_credits_pass_round", "1", "Rank", "通关所获得的积分 (积分)", "1", "30"),
                new Variable("rank_ze_win_points_humans", "", "Rank", "通关获得多少云点<人类>", "1", "30"),

                // Boss HP
                //new Variable("ze_bosshp_boss_money_bonus", "10", "BossHP", "每次攻击BOSS时获得的金钱 ($)", "1", "100"),
                new Variable("ze_bosshp_display_breakable", "0", "BossHP", "显示可破坏实体的HP (开关)", "0", "1"),
                new Variable("ze_bosshp_vscript_creation", "0", "BossHP", "地图有使用Vscript创建counter (开关)", "0", "1"),

                // entWatch
                new Variable("ze_newbee_protection_point", "10", "entWatch", "拾取神器所需的最低云点 (云点)", "500", "10000"),
                new Variable("ze_entwatch_require_client", "0", "entWatch", "xXx: DO NOT CHANGE THIS!!!", "0", "1"),

                // Glows
                new Variable("ze_buttons_glow_enabled", "1", "Glows", "按钮创建高亮透视鸡 (开关)", "0", "1"),

                // Collision
                //new Variable("ze_collision_checks", "0", "Collision", "根据距离自动识别适配Collision效果<该功能会带来CPU消耗> (开关)", "0", "1"),

                // Hide
                //new Variable("ze_extended_hide_entwatch_player", "1", "Hide", "持有神器的玩家会被透明化, 并且在僵尸视野中高亮并透视 (开关)", "0", "1"),

                // Flash Nvg
                new Variable("k_nv_enabled", "1", "FlashNvg", "夜视仪控制 (开关)", "0", "1"),
                new Variable("k_fl_enabled", "1", "FlashNvg", "手电筒控制 (开关)", "0", "1"),

                // MapMusic
                //new Variable("mapmusic_min_length", "15.0", "MapMusic", "多少时长以上的音频会被判断为地图BGM, 低于的识别为音效 (秒)", "0.0", "30.0"),

                // Save Level
                new Variable("ze_savelevel_enable", "0", "SaveLevel", "保存地图分数 (开关)", "0", "1"),
                new Variable("ze_savelevel_filter", "1", "SaveLevel", "分数过滤整数 (开关)", "0", "1"),
                new Variable("ze_savelevel_onuser", "0", "SaveLevel", "检测OnUser输出 (开关)", "0", "1"),
                new Variable("ze_savelevel_multis", "0", "SaveLevel", "多重OnUser输出 (开关)", "0", "1"),

                // Speed Mod
                //new Variable("ze_speedmod_prefab_human", "280", "SpeedMod", "人类地速限制 (Unit)", "150", "3500"),
                //new Variable("ze_speedmod_prefab_zombi", "300", "SpeedMod", "僵尸地速限制 (Unit)", "150", "3500"),

                // Map Text
                //new Variable("ze_maptext_maxcountdown", "90.0", "MapText", "最大倒计时时间<超过将转换为chat> (秒)", "5.0", "3600.0"),
                //new Variable("ze_maptext_textholdtime", "10.0", "MapText", "文本在Hud上停留的时间 (秒)", "1.0", "3600.0"),

                // User Message
                //new Variable("ze_usermessage_shake", "0", "UserMessage", "地图晃动 (开关)", "0", "1"),

                // Grenade
                //new Variable("ze_grenade_nade_duration", "1.0", "Grenade", "高爆持续时间<燃烧模式为燃烧时间, 减速模式为减速时间, 击退模式无效> (秒)", "0.0", "60.0"),
                //new Variable("ze_grenade_nade_cfthrust", "420", "Grenade", "高爆击退模式的击退力度 (Unit)", "0.0", "9999.9"),
                new Variable("ze_grenade_nade_cfeffect", "1", "Grenade", "高爆模式, 0为禁用, 1 = 燃烧, 2 = 减速, 3 = 击退 (开关)", "0", "3"),

                // Weapon
                //new Variable("ze_weapons_startmoney", "8000", "Weapon", "每局开始时补给的金钱 ($)", "1", "8000"),
                new Variable("ze_weapons_awp_counts", "5", "Weapon", "每局最大可购买的Awp数量 (把)", "1", "64"),
                new Variable("ze_weapons_spawn_hegrenade", "1", "Weapon", "每局开始时补给的高爆数量 (个)", "0", "2"),
                new Variable("ze_weapons_spawn_molotov", "0", "Weapon", "每局开始时补给的火瓶数量 (个)", "0", "1"),
                new Variable("ze_weapons_spawn_decoy", "1", "Weapon", "每局开始时补给的冰冻数量 (个)", "0", "1"),
                //new Variable("ze_weapons_spawn_healshot", "0", "Weapon", "每局开始时补给的血针数量 (支)", "0", "1"),
                new Variable("ze_weapons_round_hegrenade", "3", "Weapon", "每局最多可购买的高爆数量 (个)", "-1", "15"),
                new Variable("ze_weapons_round_molotov", "1", "Weapon", "每局最多可购买的火瓶数量 (个)", "-1", "10"),
                new Variable("ze_weapons_round_decoy", "1", "Weapon", "每局最多可购买的冰冻数量 (个)", "-1", "10"),
                new Variable("ze_weapons_round_flash", "1", "Weapon", "每局最多可购买的屏障数量 (个)", "-1", "10"),
                //new Variable("ze_weapons_round_smoke", "1", "Weapon", "每局最多可购买的磁暴数量 (个)", "-1", "5"),
                new Variable("ze_weapons_round_tagrenade", "1", "Weapon", "每局最多可购买的黑洞数量 (个)", "-1", "5"),
                new Variable("ze_weapons_round_healshot", "1", "Weapon", "每局最多可购买的血针数量 (支)", "-1", "5"),

                // ZSkill
                //new Variable("sm_hunter_enabled", "1", "ZSkill", "闪灵技能 (开关)", "0", "1"),
                //new Variable("sm_faster_enabled", "1", "ZSkill", "加速技能 (开关)", "0", "1"),
                //new Variable("sm_boomer_enabled", "1", "ZSkill", "唾沫技能 (开关)", "0", "1"),
                //new Variable("sm_smoker_enabled", "1", "ZSkill", "勾搭技能 (开关)", "0", "1"),
                //new Variable("sm_blader_enabled", "1", "ZSkill", "刀锋技能 (开关)", "0", "1"),
                //new Variable("sm_farter_enabled", "1", "ZSkill", "屁王技能 (开关)", "0", "1"),
                new Variable("sm_hunter_leappower", "300.0", "ZSkill", "闪灵冲刺推力 (Unit)", "150.0", "500.0"),
                new Variable("sm_faster_maxspeed", "1.4", "ZSkill", "加速暴发冲力 (%)", "1.1", "2.0"),
                new Variable("sm_boomer_distance", "300.0", "ZSkill", "唾液射程 (Unit)", "100.0", "999.9"),
                new Variable("sm_smoker_distance", "500.0", "ZSkill", "勾搭范围 (Unit)", "100.0", "9999.9"),
                new Variable("sm_blader_damage", "60.0", "ZSkill", "跳刀伤害 (Unit)", "30.0", "5000.0"),
                new Variable("sm_farter_distance", "350.0", "ZSkill", "毒烟半径 (Unit)", "50.0", "9999.9"),
            };

            //new Variable("", "", "", "", "", ""),
        }

        #endregion

    }
}
