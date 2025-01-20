using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Atomic.AI
{
    internal static class BlackboardAPIGenerator
    {
        private const string IMPORT_NAMESPACE = "Atomic.AI";
        private const string BLACKBOARD_CLASS = "IBlackboard";

        internal static void Generate(BlackboardConfig config,  bool refresh = true)
        {
            string directoryPath = config.directoryPath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string className = config.className;
            string filePath = config.directoryPath + "/" + className + ".cs";

            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine("/**");
            writer.WriteLine("* Code generation. Don't modify! ");
            writer.WriteLine(" */");
            
            writer.WriteLine("using System.Runtime.CompilerServices;");
            writer.WriteLine($"using {IMPORT_NAMESPACE};");

            foreach (string ns in config.imports)
            {
                writer.WriteLine($"using {ns};");
            }
            
            writer.WriteLine($"namespace {config.Namespace}");
            writer.WriteLine("{");
            writer.WriteLine($"    public static class {className}");
            writer.WriteLine("    {");

            IReadOnlyList<BlackboardConfig.Key> keys = config.Keys;

            //Generate consts:
            for (int i = 1, count = keys.Count; i < count; i++)
            {
                BlackboardConfig.Key key = keys[i];
                string keyName = key.name;
                ushort keyID = key.id;
                string keyType = string.IsNullOrEmpty(key.type) ? "Undefined" : key.type;

                writer.WriteLine($"        public const int {keyName} = {keyID}; // {keyType}");
            }

            writer.WriteLine();
            writer.WriteLine();

            //Generate extensions:
            writer.WriteLine("        ///Extensions");
            for (int i = 0, count = keys.Count; i < count; i++)
            {
                BlackboardConfig.Key key = keys[i];
                string itemName = key.name;
                string itemType = key.type;

                if (string.IsNullOrEmpty(itemType))
                {
                    continue;
                }

                GenerateTypedExtensions(writer, itemType, itemName);

                if (i < count - 1)
                {
                    writer.WriteLine();
                }
            }


            writer.WriteLine("    }");
            writer.WriteLine("}");

            AssetDatabase.SaveAssets();

            if (refresh)
            {
                AssetDatabase.Refresh();
            }
        }

        private static void GenerateTypedExtensions(StreamWriter writer, string itemType, string itemName)
        {
            //Tag
            if (itemType is "Tag" or "tag")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasTag({itemName});");
                writer.WriteLine();

                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelTag({itemName});");
                writer.WriteLine();

                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj) => obj.SetTag({itemName});");
                writer.WriteLine();
            }

            //Bool
            if (itemType is "bool" or "Bool")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasBool({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetBool({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out bool value) => obj.TryGetBool({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, bool value) => obj.SetBool({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelBool({itemName});");
                writer.WriteLine();
            }

            //Int
            if (itemType is "int" or "Int" or "Int32")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasInt({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static int Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetInt({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out int value) => obj.TryGetInt({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, int value) => obj.SetInt({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelInt({itemName});");
                writer.WriteLine();
            }

            //Float
            if (itemType is "float" or "Single" or "Float")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasFloat({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static float Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetFloat({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out float value) => obj.TryGetFloat({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, float value) => obj.SetFloat({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelFloat({itemName});");
                writer.WriteLine();
            }

            //Float2
            if (itemType is "float2" or "Vector2")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasFloat2({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static float2 Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetFloat2({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out float2 value) => obj.TryGetFloat2({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, float2 value) => obj.SetFloat2({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelFloat2({itemName});");
                writer.WriteLine();
            }

            //Float3
            if (itemType is "float3" or "Vector3")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasFloat3({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static float3 Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetFloat3({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out float3 value) => obj.TryGetFloat3({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, float3 value) => obj.SetFloat3({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelFloat3({itemName});");
                writer.WriteLine();
            }


            //Quaternion
            if (itemType is "quaternion" or "Quaternion")
            {
                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasQuaternion({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static quaternion Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetQuaternion({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out float3 value) => obj.TryGetQuaternion({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, quaternion value) => obj.SetQuaternion({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelQuaternion({itemName});");
                writer.WriteLine();
            }

            //Object
            if (itemType.EndsWith("class") || itemType.EndsWith("Class"))
            {
                itemType = itemType.Split(":")[0];

                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasObject({itemName});");
                writer.WriteLine();

                //Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static {itemType} Get{itemName}(this {BLACKBOARD_CLASS} obj) => obj.GetObject<{itemType}>({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out {itemType} value) => obj.TryGetObject({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, {itemType} value) => obj.SetObject({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelObject({itemName});");
                writer.WriteLine();
            }

            //Struct
            if (itemType.EndsWith("struct") || itemType.EndsWith("Strunct"))
            {
                itemType = itemType.Split(":")[0];

                //Has:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Has{itemName}(this {BLACKBOARD_CLASS} obj) => obj.HasStruct({itemName});");
                writer.WriteLine();

                //Get Ref:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static ref {itemType} Get{itemName}(this {BLACKBOARD_CLASS} obj) => ref obj.GetStruct<{itemType}>({itemName});");
                writer.WriteLine();

                //Try Get:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool TryGet{itemName}(this {BLACKBOARD_CLASS} obj, out Ref<{itemType}> value) => obj.TryGetStruct({itemName}, out value);");
                writer.WriteLine();

                //Set:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static void Set{itemName}(this {BLACKBOARD_CLASS} obj, {itemType} value) => obj.SetStruct({itemName}, value);");
                writer.WriteLine();

                //Del:
                writer.WriteLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                writer.WriteLine(
                    $"\t\tpublic static bool Del{itemName}(this {BLACKBOARD_CLASS} obj) => obj.DelObject({itemName});");
                writer.WriteLine();
            }
        }
    }
}