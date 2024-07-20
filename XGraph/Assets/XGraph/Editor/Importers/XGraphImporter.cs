using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

using UnityEditor;

namespace XGraph
{
    [UnityEditor.AssetImporters.ScriptedImporter(31, Extension, 3)]
    public class XGraphImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public const string Extension = "xgh";

     public const string k_ErrorShader = @"
Shader ""Hidden/GraphErrorShader2""
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
            #include ""UnityCG.cginc""

            struct appdata_t {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1,0,1,1);
            }
            ENDCG
        }
    }
    Fallback Off
}";

        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
        {
            var oldShader = AssetDatabase.LoadAssetAtPath<Shader>(ctx.assetPath);
            // if (oldShader != null)
            //     ShaderUtil.ClearShaderMessages(oldShader);

            List<PropertyCollector.TextureInfo> configuredTextures;
            string path = ctx.assetPath;
            var sourceAssetDependencyPaths = new List<string>();

            UnityEngine.Object mainObject;

            var textGraph = File.ReadAllText(path, Encoding.UTF8);
            GraphData graph = JsonUtility.FromJson<GraphData>(textGraph);
            graph.messageManager = new MessageManager();
            graph.assetGuid = AssetDatabase.AssetPathToGUID(path);
            graph.OnEnable();
            graph.ValidateGraph();

            // if (graph.outputNode is VfxMasterNode vfxMasterNode)
            // {
            //     var vfxAsset = GenerateVfxShaderGraphAsset(vfxMasterNode);
                
            //     mainObject = vfxAsset;
            // }
            // else
            // {
            //     var text = GetShaderText(path, out configuredTextures, sourceAssetDependencyPaths,graph);
            //     var shader = ShaderUtil.CreateShaderAsset(text, false);

            //     if (graph != null && graph.messageManager.nodeMessagesChanged)
            //     {
            //         foreach (var pair in graph.messageManager.GetNodeMessages())
            //         {
            //             var node = graph.GetNodeFromTempId(pair.Key);
            //             MessageManager.Log(node, path, pair.Value.First(), shader);
            //         }
            //     }

            //     EditorMaterialUtility.SetShaderDefaults(
            //         shader,
            //         configuredTextures.Where(x => x.modifiable).Select(x => x.name).ToArray(),
            //         configuredTextures.Where(x => x.modifiable).Select(x => EditorUtility.InstanceIDToObject(x.textureId) as Texture).ToArray());
            //     EditorMaterialUtility.SetShaderNonModifiableDefaults(
            //         shader,
            //         configuredTextures.Where(x => !x.modifiable).Select(x => x.name).ToArray(),
            //         configuredTextures.Where(x => !x.modifiable).Select(x => EditorUtility.InstanceIDToObject(x.textureId) as Texture).ToArray());

            //     mainObject = shader;
            // }
            // Texture2D texture = Resources.Load<Texture2D>("Icons/sg_graph_icon@64");
            // ctx.AddObjectToAsset("MainAsset", mainObject, texture);
            // ctx.SetMainObject(mainObject);

            // var metadata = ScriptableObject.CreateInstance<ShaderGraphMetadata>();
            // metadata.hideFlags = HideFlags.HideInHierarchy;
            // if (graph != null)
            // {
            //     metadata.outputNodeTypeName = graph.outputNode.GetType().FullName;
            // }
            // ctx.AddObjectToAsset("Metadata", metadata);

            // foreach (var sourceAssetDependencyPath in sourceAssetDependencyPaths.Distinct())
            // {
            //     // Ensure that dependency path is relative to project
            //     if (!sourceAssetDependencyPath.StartsWith("Packages/") && !sourceAssetDependencyPath.StartsWith("Assets/"))
            //     {
            //         Debug.LogWarning($"Invalid dependency path: {sourceAssetDependencyPath}", mainObject);
            //         continue;
            //     }

            //     ctx.DependsOnSourceAsset(sourceAssetDependencyPath);
            // }
        }

        internal static string GetShaderText(string path, out List<PropertyCollector.TextureInfo> configuredTextures, List<string> sourceAssetDependencyPaths, GraphData graph)
        {
            string shaderString = null;
            var shaderName = Path.GetFileNameWithoutExtension(path);
            try
            {
                if (!string.IsNullOrEmpty(graph.path))
                    shaderName = graph.path + "/" + shaderName;
                shaderString = ((IMasterNode)graph.outputNode).GetShader(GenerationMode.ForReals, shaderName, out configuredTextures, sourceAssetDependencyPaths);

                if (graph.messageManager.nodeMessagesChanged)
                {
                    shaderString = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                configuredTextures = new List<PropertyCollector.TextureInfo>();

                // ignored
            }

            return shaderString ?? k_ErrorShader.Replace("Hidden/GraphErrorShader2", shaderName);
        }
    }
}