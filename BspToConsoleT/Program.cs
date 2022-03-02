using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BspToConsoleT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("请直接把bsp地图文件拖到App上运行!");
                Console.ReadKey(true);
                Environment.Exit(1);
            }

            var file = args[0];

            if (!File.Exists(file))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("文件无法访问!");
                Console.ReadKey(true);
                Environment.Exit(1);
            }

            try
            {
                Console.Title = "Bsp翻译文件导出 by Kyle -> " + Path.GetFileNameWithoutExtension(file);

                using var vbsp = new Bsp(file);

                var lump = await vbsp.ReadLumps(0);

                File.WriteAllBytes(file.Replace(".bsp", ".lump"), lump);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"已解析 {Path.GetFileNameWithoutExtension(file)}.bsp >>> {Path.GetFileNameWithoutExtension(file)}.lump");

                var text = Encoding.UTF8.GetString(lump);
                var list = i18n.ParseLump(text);
                i18n.Dump(file.Replace(".bsp", ".txt"), list);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"已解析 {Path.GetFileNameWithoutExtension(file)}.bsp >>> {Path.GetFileNameWithoutExtension(file)}.text -> {list.Count} 条翻译.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"发生异常错误: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{Environment.NewLine}");
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}
