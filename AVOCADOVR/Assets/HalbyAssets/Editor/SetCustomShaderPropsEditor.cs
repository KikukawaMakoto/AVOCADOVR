using System.Linq;
using System.Text.RegularExpressions;
using DigitalOpus.MB.Core;
using UniGLTF.ShaderPropExporter;
using UnityEditor;

namespace HALBY.Utility
{
    public class SetShaderPropsEditor : Editor
    {
        [MenuItem("CONTEXT/MB3_TextureBaker/Auto Assign Prop Names")]
        private static void AutoAssignPropNames(MenuCommand command)
        {
            var baker = command.context as MB3_TextureBaker;
            if (baker == null) return;
            Undo.RecordObject(baker.gameObject, "Assign Shader Prop Names");

            var materials = baker.resultMaterials.SelectMany(m => m.sourceMaterials).Distinct().ToList();
            // シェーダーが持つ全テクスチャプロパティを列挙する
            var texProps = materials.Select(m => m.shader).Distinct()
                .ToDictionary(s => s.name, s => ShaderProps.FromShader(s).Properties
                    .Where(p => p.ShaderPropertyType == ShaderPropertyType.TexEnv));
            // 使われているマテリアルが所持しているテクスチャプロパティを列挙する
            var usedShaderProps = materials
                .GroupBy(m => m.shader.name)
                // 同一シェーダーのすべてのマテリアルにおいてテクスチャが1つも入ってないプロパティを除外する
                .ToDictionary(g => g.Key, g => texProps[g.Key]
                    // ここでは該当シェーダの全テクスチャプロパティからGetTextureの戻り値がすべてnullになるものをExceptしている
                    .Except(texProps[g.Key].Where(p => g.Select(m => m.GetTexture(p.Key)).All(t => t == null)))
                    .Select(p => p.Key))
                .SelectMany(p => p.Value).Distinct();

            baker.customShaderProperties =
                usedShaderProps.Select(p =>
                        new ShaderTextureProperty(p, Regex.IsMatch(p, "Normal|Bump", RegexOptions.IgnoreCase)))
                    .ToList();
        }
    }
}