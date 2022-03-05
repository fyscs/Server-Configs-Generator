using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigReFormatter
{
    internal class Program
    {
        static readonly string myself = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static readonly string worker = Path.Combine(myself, "worker");
        static readonly string output = Path.Combine(myself, "output");

        static void Main(string[] args)
        {
            Console.Title = "Configs Re-Formatter by Kyle";

            Directory.CreateDirectory(worker);
            Directory.CreateDirectory(output);

            Directory.GetFiles(worker, "*.*", SearchOption.TopDirectoryOnly).ToList().ForEach(conf =>
            {
                try
                {
                    ParseConfig(conf);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to parse file {conf} => {e.Message}{Environment.NewLine}{e.StackTrace}");
                }
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done");
            Console.ReadKey(true);
        }

        private static void ParseConfig(string path)
        {
            var text = File.ReadAllText(path);

            var vdf = VdfConvert.Deserialize(text);

            switch (vdf.Key.ToLower())
            {
                case "entities":
                    ParseEntity(path, vdf);
                    break;
                case "bosshp":
                    ParseBossHp(path, vdf);
                    break;
                case "buttons":
                    ParseButton(path, vdf);
                    break;
                default:
                    throw new NotImplementedException($"undefined config type '{vdf.Key}' in [{path}]");
            }
        }

        private static void ParseButton(string path, VProperty vdf)
        {
            var config = vdf.Value.ToJson().ToObject<Dictionary<string, Button>>();

            var text = "\"Buttons\"" + Environment.NewLine + "{" + Environment.NewLine;

            text += "    // Button configuration file generator." + Environment.NewLine;
            text += "    // Copyright 2022 Kyle 'Kxnrl' Frankiss " + Environment.NewLine;
            text += "    // https://github.com/fys-csgo/Server-Configs-Generator" + Environment.NewLine;
            text += Environment.NewLine;

            var index = 0;
            config.Values.ToList().ForEach(button =>
            {
                text += Environment.NewLine;
                text += "    " + "\"" + (++index) + "\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;

                // adjust
                if (button.Mode < -1 || button.Mode > 9)
                    button.Mode = -1;

                if (button.Cooldown > 1800)
                    button.Cooldown = 1800;
                if (button.Cooldown < 360)
                    button.Cooldown = 360;

                text += $"        \"id\"    \"{button.HammerId}\"" + Environment.NewLine;
                text += $"        \"cd\"    \"{button.Cooldown}\"" + Environment.NewLine;
                text += $"        \"mode\"  \"{button.Mode}\"" + Environment.NewLine;
                text += $"        \"name\"  \"{button.Name}\"" + Environment.NewLine;

                text += "    " + "}" + Environment.NewLine;
            });

            text += Environment.NewLine + "}";

            File.WriteAllText(path.Replace(worker, output), text, new UTF8Encoding(false));
        }

        private static void ParseEntity(string path, VProperty vdf)
        {
            var config = vdf.Value.ToJson().ToObject<Dictionary<string, EntWatch>>();

            var text = "\"entities\"" + Environment.NewLine + "{" + Environment.NewLine;

            text += "    // EntWatch configuration file generator." + Environment.NewLine;
            text += "    // Copyright 2022 Kyle 'Kxnrl' Frankiss " + Environment.NewLine;
            text += "    // https://github.com/fys-csgo/Server-Configs-Generator" + Environment.NewLine;
            text += Environment.NewLine;

            var index = 0;
            config.Values.ToList().ForEach(entity =>
            {
                text += Environment.NewLine;
                text += "    " + "\"" + (++index) + "\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;

                // adjust
                if (string.IsNullOrEmpty(entity.FilterName) || "null".Equals(entity.FilterName))
                {
                    entity.FilterName = "__null__";
                    entity.HasFilter = false;
                }

                if (string.IsNullOrEmpty(entity.ButtonClass) || "null".Equals(entity.ButtonClass))
                    entity.ButtonClass = "__null__";

                // required
                {
                    text += $"        \"name\"              \"{entity.Name}\"" + Environment.NewLine;
                    text += $"        \"shortname\"         \"{entity.ShortName}\"" + Environment.NewLine;
                    text += $"        \"team\"              \"{entity.Team}\"" + Environment.NewLine;
                    text += $"        \"buttonclass\"       \"{entity.ButtonClass}\"" + Environment.NewLine;
                    text += $"        \"hasfiltername\"     \"{entity.HasFilter.ToKv()}\"" + Environment.NewLine;
                    text += $"        \"filtername\"        \"{entity.FilterName}\"" + Environment.NewLine;
                    text += $"        \"hammerid\"          \"{entity.HammerId}\"" + Environment.NewLine;
                    text += $"        \"maxamount\"         \"{entity.MaxAmount}\"" + Environment.NewLine;
                    text += $"        \"mode\"              \"{entity.Mode}\"" + Environment.NewLine;
                }

                if (entity.TriggerId.GetValueOrDefault(0) > 0)
                {
                    text += $"        \"triggerid\"         \"{entity.TriggerId}\"" + Environment.NewLine;
                }

                if (entity.ContainerId.GetValueOrDefault(0) > 0)
                {
                    text += $"        \"containerid\"       \"{entity.ContainerId}\"" + Environment.NewLine;
                }

                if (entity.Startcd.GetValueOrDefault(0) > 0)
                {
                    text += $"        \"startcd\"           \"{entity.Startcd}\"" + Environment.NewLine;
                }

                if (entity.WallEntity.HasValue)
                {
                    text += $"        \"isWall\"            \"{entity.WallEntity.ToKv()}\"" + Environment.NewLine;
                }

                if (entity.Glow.HasValue)
                {
                    text += $"        \"glow\"              \"{entity.Glow.ToKv()}\"" + Environment.NewLine;
                }

                if (entity.Hud.HasValue)
                {
                    text += $"        \"hud\"               \"{entity.Hud.ToKv()}\"" + Environment.NewLine;
                }

                if (entity.AutoTransfer.HasValue)
                {
                    text += $"        \"autotransfer\"      \"{entity.AutoTransfer.ToKv()}\"" + Environment.NewLine;
                }

                if ("player".Equals(entity.Level) || "entity".Equals(entity.Level))
                {
                    text += $"        \"level\"             \"{entity.Level}\"" + Environment.NewLine;
                }

                if (entity.Children != null)
                {
                    text += $"        \"children\"" + Environment.NewLine;
                    text += $"        " + "{" + Environment.NewLine;

                    {
                        text += $"            \"type\"          \"{entity.Children.Type}\"" + Environment.NewLine;
                        text += $"            \"name\"          \"{entity.Children.Name}\"" + Environment.NewLine;
                    }

                    text += $"            \"case\"" + Environment.NewLine;
                    text += $"            " + "{" + Environment.NewLine;
                    foreach (var part in entity.Children.Cases)
                    {
                        var spliter = "         ";
                        switch (part.Key.ToString().Length)
                        {
                            case 3:
                                spliter = "        ";
                                break;
                            case 2:
                                spliter = "        ";
                                break;
                            default:
                                spliter = "         ";
                                break;
                        }
                        text += $"                \"{part.Key}\"{spliter}\"{part.Value}\"" + Environment.NewLine;
                    }
                    text += $"            " + "}" + Environment.NewLine;


                    text += $"        " + "}" + Environment.NewLine;
                }

                if (entity.Holy != null)
                {
                    text += $"        \"holy\"" + Environment.NewLine;
                    text += $"        " + "{" + Environment.NewLine;

                    {
                        text += $"            \"triggerid\"     \"{entity.Holy.TriggerId}\"" + Environment.NewLine;
                        text += $"            \"mode\"          \"{entity.Holy.Mode}\"" + Environment.NewLine;
                    }

                    if (entity.Holy.MaxUses.GetValueOrDefault(0) > 0)
                    {
                        text += $"            \"maxuses\"       \"{entity.Holy.MaxUses}\"" + Environment.NewLine;
                    }

                    if (entity.Holy.Cooldown.GetValueOrDefault(0) > 0)
                    {
                        text += $"            \"cooldown\"      \"{entity.Holy.Cooldown}\"" + Environment.NewLine;
                    }

                    {
                        text += $"            \"class\"         \"{entity.Holy.Class}\"" + Environment.NewLine;
                        text += $"            \"event\"         \"{entity.Holy.Event}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(entity.Holy.Title))
                    {
                        text += $"            \"title\"         \"{entity.Holy.Title}\"" + Environment.NewLine;
                    }

                    text += $"        " + "}" + Environment.NewLine;
                }

                text += "    " + "}" + Environment.NewLine;
            });

            text += Environment.NewLine + "}";

            File.WriteAllText(path.Replace(worker, output), text, new UTF8Encoding(false));
        }

        private static void ParseBossHp(string path, VProperty vdf)
        {
            var config = vdf.Value.ToJson().ToObject<BossHp>();

            var text = "\"BossHP\"" + Environment.NewLine + "{" + Environment.NewLine;

            text += "    // BossHP configuration file generator." + Environment.NewLine;
            text += "    // Copyright 2022 Kyle 'Kxnrl' Frankiss " + Environment.NewLine;
            text += "    // https://github.com/fys-csgo/Server-Configs-Generator" + Environment.NewLine;
            text += Environment.NewLine;

            if (config?.Breakables?.Count > 0)
            {
                text += Environment.NewLine;
                text += "    " + "\"breakable\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;

                var index = 0;
                config.Breakables.Values.ToList().ForEach(boss =>
                {
                    text += Environment.NewLine;
                    text += "        " + "\"" + (++index) + "\"" + Environment.NewLine;
                    text += "        " + "{" + Environment.NewLine;

                    // required
                    {
                        text += $"            \"targetname\"    \"{boss.TargetName}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.DisplayName) && !boss.DisplayName.ToLower().Equals("boss"))
                    {
                        text += $"            \"displayname\"   \"{boss.DisplayName}\"" + Environment.NewLine;
                    }

                    if (boss.Count.GetValueOrDefault(0) > 0)
                    {
                        text += $"            \"hpcounts\"      \"{boss.Count}\"" + Environment.NewLine;
                    }

                    if (boss.CashOnly.HasValue)
                    {
                        text += $"            \"cashonly\"      \"{boss.CashOnly.ToKvNum()}\"" + Environment.NewLine;
                    }

                    if (boss.MultiParts.HasValue)
                    {
                        text += $"            \"multiparts\"    \"{boss.MultiParts.ToKvNum()}\"" + Environment.NewLine;
                    }

                    text += "        " + "}" + Environment.NewLine;
                });

                text += Environment.NewLine + "    " + "}" + Environment.NewLine;
            }

            if (config?.Counters?.Count > 0)
            {
                text += Environment.NewLine;
                text += "    " + "\"counter\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;

                var index = 0;
                config.Counters.Values.ToList().ForEach(boss =>
                {
                    text += Environment.NewLine;
                    text += "        " + "\"" + (++index) + "\"" + Environment.NewLine;
                    text += "        " + "{" + Environment.NewLine;

                    // required
                    {
                        text += $"            \"iterator\"      \"{boss.Iterator}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.Backup))
                    {
                        text += $"            \"backup\"        \"{boss.Backup}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.Counter))
                    {
                        text += $"            \"counter\"       \"{boss.Counter}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.DisplayName) && !boss.DisplayName.ToLower().Equals("boss"))
                    {
                        text += $"            \"displayname\"   \"{boss.DisplayName}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.HitBox))
                    {
                        text += $"            \"hitbox\"        \"{boss.HitBox}\"" + Environment.NewLine;
                    }

                    if (boss.HitMax.HasValue)
                    {
                        text += $"            \"countermode\"   \"{boss.HitMax.ToKvNum()}\"" + Environment.NewLine;
                    }

                    if (boss.MultiParts.HasValue)
                    {
                        text += $"            \"multiparts\"    \"{boss.MultiParts.ToKvNum()}\"" + Environment.NewLine;
                    }

                    text += "        " + "}" + Environment.NewLine;
                });

                text += Environment.NewLine + "    " + "}" + Environment.NewLine;
            }

            if (config?.Monsters?.Count > 0)
            {
                text += Environment.NewLine;
                text += "    " + "\"monster\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;

                var index = 0;
                config.Monsters.Values.ToList().ForEach(boss =>
                {
                    text += Environment.NewLine;
                    text += "        " + "\"" + (++index) + "\"" + Environment.NewLine;
                    text += "        " + "{" + Environment.NewLine;

                    // required
                    {
                        text += $"            \"hammerid\"      \"{boss.HammerId}\"" + Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(boss.DisplayName) && !boss.DisplayName.ToLower().Equals("boss"))
                    {
                        text += $"            \"displayname\"   \"{boss.DisplayName}\"" + Environment.NewLine;
                    }

                    text += "        " + "}" + Environment.NewLine;
                });

                text += Environment.NewLine + "    " + "}" + Environment.NewLine;
            }

            text += Environment.NewLine + "}";

            File.WriteAllText(path.Replace(worker, output), text, new UTF8Encoding(false));
        }
    }
}
