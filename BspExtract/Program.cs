using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BspExtract
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG
            var file = @"E:\Desktop\de_inferno_pro-mod.bsp";
#else
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
#endif

            try
            {
                Console.Title = "Bsp文件提取器 by Kyle -> " + Path.GetFileNameWithoutExtension(file);

                using var vbsp = new Bsp(file);

                var lump = await vbsp.ReadLumps(40);
                var path = file.Replace(".bsp", ".zip");

                File.WriteAllBytes(path, lump);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"已解析 {Path.GetFileNameWithoutExtension(file)}.bsp >>> {Path.GetFileNameWithoutExtension(file)}.zip");

                var zip = ZipFile.OpenRead(path);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"已解析 {Path.GetFileNameWithoutExtension(file)}.bsp >>> {Path.GetFileNameWithoutExtension(file)}.text -> {zip.Entries.Count} 个文件.");

                foreach (var entry in zip.Entries)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"    {entry.FullName}");
                }
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
