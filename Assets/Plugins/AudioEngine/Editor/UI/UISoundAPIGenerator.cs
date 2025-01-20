using System.Collections.Generic;
using System.IO;

namespace AudioEngine
{
    internal static class UISoundAPIGenerator
    {
        internal static void Generate(IReadOnlyList<UISoundCatalog.SoundInfo> sounds)
        {
            const string selectedPath = "Assets/Plugins/AudioEngine/Codegen/UI/UISoundAPI.cs";

            using StreamWriter writer = new StreamWriter(selectedPath);
            
            writer.WriteLine("/**");
            writer.WriteLine("* Code generation. Don't modify! ");
            writer.WriteLine(" */");
            
            writer.WriteLine();
            
            writer.WriteLine("namespace AudioEngine");
            writer.WriteLine("{");
            writer.WriteLine("    public static class UISoundAPI");
            writer.WriteLine("    {");

            for (int i = 0, count = sounds.Count; i < count; i++)
            {
                UISoundCatalog.SoundInfo sound = sounds[i];
                string soundID = sound.id;
                
                writer.WriteLine($"        public const string {soundID.ToUpper()} = \"{soundID}\";");
            }
            
            writer.WriteLine("    }");
            writer.WriteLine("}");
        }
    }
}