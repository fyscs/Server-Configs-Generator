using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BspToConsoleT
{
    public class i18n
    {
        public static List<string> ParseLump(string lump)
        {
            var text = lump.Split('\n');
            var list = new List<string>();

            //"OnTrigger" "serverCommandsay ***YOU TOOK TOO LONG - NUKING HUMANS***0-1"
            //"OnTrigger" "boss_attack_caseAddOutputOnCase07 servercommand:Command:say ***Rock monster will use backward-push in 3s!***:0.00:0 01"

            text.ToList().ForEach(current =>
            {
                var line = string.Copy(current);

                // is not say hook
                if (!line.Contains("Command") || !line.Contains("say "))
                {
                    // skip line
                    return;
                }

                if (line.Contains("sm_say"))
                    line = line.Replace("sm_say", "say");

                var raw = line.Split(new[] { "\" \"" }, StringSplitOptions.None);
                if (raw.Length != 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parse line key values -> [{current}]");
                    return;
                }

                line = raw[1].Substring(0, raw[1].Length - 1);

                var action = SplitEntityAction(line.Substring(1, line.Length - 2));

                if (action.Length != 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parse line to action -> [{current}]");
                    return;
                }

                var text = action[2];

                if (action[1].ToLower().Equals("addoutput"))
                    text = PraseOutput(action[2].TrimEnd());

                if (string.IsNullOrEmpty(text))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error parse line to command -> [{current}]");
                    return;
                }

                list.Add(text.Substring(4).Trim());
            });

            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("can not find any translation line.");
                return new List<string>();
            }

            return list.Distinct().OrderBy(x => x).ToList();
        }

        private static string PraseOutput(string line)
        {
            var firstSpace = line.IndexOf(' ');

            var targetname = line.Substring(0, firstSpace-1).Trim();
            var param = line.Substring(firstSpace).Trim();

            var action = SplitEntityAction(param); //param.Split(new char[] { delimiter }, StringSplitOptions.None);
            if (action.Length == 0)
                return string.Empty;

            return action[2];
        }

        private static string[] SplitEntityAction(string thval)
        {
            var delimiters = new char[] {'', ',', ';', ':'};

            foreach (var delimiter in delimiters)
            {
                var text = thval.Split(new char [] { delimiter }, StringSplitOptions.None);
                if (text.Length == 5)
                    return text;
            }

            return new string[] { };
        }

        public static void Dump(string path, List<string> list)
        {
            var text = "\"Console_T\"" + Environment.NewLine + "{" + Environment.NewLine;

            text += "    // Console SayText i18n file generator." + Environment.NewLine;
            text += "    // Copyright 2022 Kyle 'Kxnrl' Frankiss " + Environment.NewLine;
            text += "    // https://github.com/fys-csgo/Server-Configs-Generator" + Environment.NewLine;
            text += Environment.NewLine;

            text += "    // 可用字段" + Environment.NewLine;
            text += "    // \"blocked\"    // 屏蔽本句输出" + Environment.NewLine;
            text += "    // \"cleartext\"  // 清除所有HUD文本" + Environment.NewLine;
            text += "    // \"cleartimer\" // 清除所有倒计时" + Environment.NewLine;
            text += "    // \"countdown\"  // 添加特殊的独立的倒计时" + Environment.NewLine;
            text += Environment.NewLine;
            //text += "    // \"command\"    // 服务器执行指令" + Environment.NewLine;

            list.ForEach(line =>
            {
                text += Environment.NewLine;
                text += "    " + "\"" + line + "\"" + Environment.NewLine;
                text += "    " + "{" + Environment.NewLine;
                text += "    " + "    " + "\"chi\"" + " \"" + "__null__" + "\"" + Environment.NewLine;
                text += "    " + "}" + Environment.NewLine;

            });

            text += Environment.NewLine + "}";

            File.WriteAllText(path, text, new UTF8Encoding(false));
        }
    }
}
