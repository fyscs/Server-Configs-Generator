using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kxnrl.FyS.VmfToConsoleT
{
    class Program
    {
        static readonly string myself = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        static readonly string worker = Path.Combine(myself, "worker");
        static readonly string output = Path.Combine(myself, "output");

        #region FGD Outputs
        static readonly List<string> outputs = new List<string>
        {
            "onuse",
            "onroundended",
            "onignite",
            "onuser1",
            "onuser2",
            "onuser3",
            "onuser4",
            "counter",
            "onbreak",
            "ontakedamage",
            "onhealthchanged",
            "onphyscannondetach",
            "onphyscannonanimateprestarted",
            "onphyscannonanimatepullstarted",
            "onphyscannonpullanimfinished",
            "onphyscannonanimatepoststarted",
            "ondamaged",
            "ondeath",
            "onhalfhealth",
            "onhearworld",
            "onhearplayer",
            "onhearcombat",
            "onfoundenemy",
            "onlostenemylos",
            "onlostenemy",
            "onfoundplayer",
            "onlostplayerlos",
            "onlostplayer",
            "ondamagedbyplayer",
            "ondamagedbyplayersquad",
            "ondenycommanderuse",
            "onsleep",
            "onwake",
            "onforcedinteractionstarted",
            "onforcedinteractionaborted",
            "onforcedinteractionfinished",
            "onspawnnpc",
            "onspawnnpc",
            "onallspawned",
            "onallspawneddead",
            "onalllivechildrendead",
            "onstarttouch",
            "onstarttouchall",
            "onendtouch",
            "onendtouchall",
            "onbeginfade",
            "onsurfacechangedtotarget",
            "onsurfacechangedfromtarget",
            "onplayergotonladder",
            "onplayergotoffladder",
            "ontouchedbyentity",
            "onpushedplayer",
            "onignited",
            "onextinguished",
            "onheatlevelstart",
            "onheatlevelend",
            "onshowmessage",
            "onplay",
            "onguststart",
            "ongustend",
            "onlighton",
            "onlightoff",
            "playeron",
            "playeroff",
            "pressedmoveleft",
            "pressedmoveright",
            "pressedforward",
            "pressedback",
            "pressedattack",
            "pressedattack2",
            "unpressedmoveleft",
            "unpressedmoveright",
            "unpressedforward",
            "unpressedback",
            "unpressedattack",
            "unpressedattack2",
            "xaxis",
            "yaxis",
            "attackaxis",
            "attack2axis",
            "onfoundentity",
            "onplayerinzone",
            "onplayeroutzone",
            "playersincount",
            "playersoutcount",
            "onnpcstartedusing",
            "onnpcstoppedusing",
            "onfullyopen",
            "onfullyclosed",
            "onfullyopen",
            "onfullyclosed",
            "ongetspeed",
            "ondamaged",
            "onpressed",
            "onuselocked",
            "onin",
            "onout",
            "position",
            "onpressed",
            "onunpressed",
            "onfullyclosed",
            "onfullyopen",
            "onreachedposition",
            "onclose",
            "onopen",
            "onfullyopen",
            "onfullyclosed",
            "onblockedclosing",
            "onblockedopening",
            "onunblockedclosing",
            "onunblockedopening",
            "onlockeduse",
            "onclose",
            "onopen",
            "onfullyopen",
            "onfullyclosed",
            "onblockedclosing",
            "onblockedopening",
            "onunblockedclosing",
            "onunblockedopening",
            "onlockeduse",
            "onrotationdone",
            "onmapspawn",
            "onnewgame",
            "onloadgame",
            "onmaptransition",
            "onbackgroundmap",
            "onmultinewmap",
            "onmultinewround",
            "onendfollow",
            "onlessthan",
            "onequalto",
            "onnotequalto",
            "ongreaterthan",
            "ontrue",
            "onfalse",
            "onalltrue",
            "onallfalse",
            "onmixed",
            "oncase01",
            "oncase02",
            "oncase03",
            "oncase04",
            "oncase05",
            "oncase06",
            "oncase07",
            "oncase08",
            "oncase09",
            "oncase10",
            "oncase11",
            "oncase12",
            "oncase13",
            "oncase14",
            "oncase15",
            "oncase16",
            "ondefault",
            "onequal",
            "onnotequal",
            "onspawn",
            "ontrigger",
            "onregisteredactivate1",
            "onregisteredactivate2",
            "onregisteredactivate3",
            "onregisteredactivate4",
            "onspawn",
            "ontrigger1",
            "ontrigger2",
            "ontrigger3",
            "ontrigger4",
            "ontrigger5",
            "ontrigger6",
            "ontrigger7",
            "ontrigger8",
            "ontimer",
            "ontimerhigh",
            "ontimerlow",
            "soundlevel",
            "onroutedsound",
            "onheardsound",
            "outvalue",
            "outcolor",
            "outvalue",
            "onhitmin",
            "onhitmax",
            "onchangedfrommin",
            "onchangedfrommax",
            "ongetvalue",
            "line",
            "onplaybackfinished",
            "onentityspawned",
            "onentityspawned",
            "onentityfailedspawn",
            "onpass",
            "onfail",
            "targetdir",
            "onfacinglookat",
            "onnotfacinglookat",
            "facingpercentage",
            "angularvelocity",
            "ongreaterthan",
            "ongreaterthanorequalto",
            "onlessthan",
            "onlessthanorequalto",
            "onequalto",
            "velocity",
            "onconstraintbroken",
            "ondamaged",
            "onawakened",
            "onmotionenabled",
            "onphysgunpickup",
            "onphysgunpunt",
            "onphysgunonlypickup",
            "onphysgundrop",
            "onplayeruse",
            "onbreak",
            "onactivate",
            "onawakened",
            "onconvert",
            "onattach",
            "ondetach",
            "onanimationbegun",
            "onanimationdone",
            "onmotionenabled",
            "onawakened",
            "onphysgunpickup",
            "onphysgunpunt",
            "onphysgunonlypickup",
            "onphysgundrop",
            "onplayeruse",
            "onplayerpickup",
            "onoutofworld",
            "ondeath",
            "onstart",
            "onnext",
            "onarrivedatdestinationnode",
            "ondeath",
            "onpass",
            "onchangelevel",
            "onhurt",
            "onhurtplayer",
            "onremove",
            "ontrigger",
            "ontouching",
            "onnottouching",
            "ontrigger",
            "ontrigger",
            "ontimeout",
            "impactforce",
            "nearestentitydistance",
            "oncreatenpc",
            "onfailedtocreatenpc",
            "oncreateaddon",
            "onfailedtocreateaddon",
            "oneventfired",
            "oneventfired",
            "oncreditsdone",
            "onstartslowingtime",
            "onstopslowingtime",
            "onprimaryportalplaced",
            "onsecondaryportalplaced",
            "onduck",
            "onunduck",
            "onjump",
            "onflashlighton",
            "onflashlightoff",
            "playerhealth",
            "playermissedar2altfire",
            "playerhasammo",
            "playerhasnoammo",
            "playerdied",
            "onlighton",
            "onlightoff",
            "onproxyrelay",
            "onfire",
            "onaquiretarget",
            "onlosetarget",
            "onammodepleted",
            "ongotcontroller",
            "onlostcontroller",
            "ongotplayercontroller",
            "onlostplayercontroller",
            "onreadytofire",
            "onuse",
            "onroundended"
        };
        #endregion

        static void Main(string[] args)
        {
            Console.Title = "Vmf say command string export tool by Kyle";

            Directory.CreateDirectory(worker);
            Directory.CreateDirectory(output);

            Directory.GetFiles(worker, "*.vmf", SearchOption.TopDirectoryOnly).ToList().ForEach(vmf =>
            {
                try
                {
                    ParseVmf(vmf);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to parse file {vmf} => {e.Message}{Environment.NewLine}{e.StackTrace}");
                }
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done");
            Console.ReadKey(true);
        }

        private static void ParseVmf(string path)
        {
            var list = new List<string>();
            //text = thval.Substring(1, thval.Length - 2).Split(new[] { "" }, StringSplitOptions.None); // 0x1b
            //text = thval.Substring(1, thval.Length - 2).Split(new[] { ";" }, StringSplitOptions.None); // 0x1b
            //text = thval.Substring(1, thval.Length - 2).Split(new[] { "," }, StringSplitOptions.None);
            File.ReadAllLines(path, Encoding.UTF8).ToList().ForEach(line =>
            {
                // is not say hook
                if (!line.Contains("Command") || !line.Contains("say "))
                {
                    // skip line
                    return;
                }

                //"OnTrigger" "serverCommandsay ***YOU TOOK TOO LONG - NUKING HUMANS***0-1"

                var raw = line.Split(new[] { "\" \"" }, StringSplitOptions.None);
                if (raw.Length != 2)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[{1}] Error parse line key values -> [{0}]", line, Path.GetFileNameWithoutExtension(path));
                    return;
                }

                //  serverCommandsay ***YOU TOOK TOO LONG - NUKING HUMANS***0-1
                line = raw[1].Substring(0, raw[1].Length - 1);

                var action = SplitEntityAction(line);

                if (action.Length != 5)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[{1}] Error parse line to action -> [{0}]", line, Path.GetFileNameWithoutExtension(path));
                    return;
                }

                list.Add(action[2].Substring(4));
            });

            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Map [{Path.GetFileNameWithoutExtension(path)}] can not find any translation line.");
                return;
            }

            File.WriteAllText(Path.Combine(output, Path.GetFileNameWithoutExtension(path) + ".txt"), 
                TranslationsToKeyValues(list), 
                new UTF8Encoding(false));
        }

        private static string[] SplitEntityAction(string thval)
        {
            string[] text;

            text = thval.Substring(1, thval.Length - 2).Split(new[] { "" }, StringSplitOptions.None); // 0x1b
            if (text.Length == 5)
                return text;

            text = thval.Substring(1, thval.Length - 2).Split(new[] { "," }, StringSplitOptions.None);
            if (text.Length == 5)
                return text;

            text = thval.Substring(1, thval.Length - 2).Split(new[] { ";" }, StringSplitOptions.None);
            if (text.Length == 5)
                return text;

            return new string[] { };
        }

        private static string TranslationsToKeyValues(List<string> transTxt)
        {
            var text = "\"Console_T\"" + Environment.NewLine + "{" + Environment.NewLine;

            text += "    // 可用字段" + Environment.NewLine;
            text += "    // \"blocked\" // 屏蔽本句输出" + Environment.NewLine;
            text += "    // \"command\" // 服务器执行指令" + Environment.NewLine;
            text += "    // \"countdown\" // 添加特殊的独立的倒计时" + Environment.NewLine;

            transTxt.Distinct().ToList().ForEach(line =>
            {
                text += Environment.NewLine;
                text += "    " + "\"" + line + "\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;
                text += "    " + "    " + "\"chi\"" + " \"" + "** **" + "\"" + Environment.NewLine;
                text += "    " + "}" + Environment.NewLine;

            });

            text += Environment.NewLine + "}";
            return text;
        }
    }
}
