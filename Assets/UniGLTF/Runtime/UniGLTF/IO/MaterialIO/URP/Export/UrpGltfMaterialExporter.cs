using UnityEngine;

namespace UniGLTF
{
    public class UrpGltfMaterialExporter : IMaterialExporter
    {
        public UrpLitMaterialExporter UrpLitExporter { get; set; } = new UrpLitMaterialExporter();
        public UrpUnlitMaterialExporter UrpUnlitExporter { get; set; } = new UrpUnlitMaterialExporter();
        public UrpUniUnlitMaterialExporter UrpUniUnlitExporter { get; set; } = new UrpUniUnlitMaterialExporter();
        public UrpFallbackMaterialExporter FallbackExporter { get; set; } = new UrpFallbackMaterialExporter();

        public glTFMaterial ExportMaterial(Material m, ITextureExporter textureExporter, GltfExportSettings settings)
        {
            if (UrpLitExporter.TryExportMaterial(m, textureExporter, out var dst)) return dst;
            if (UrpUnlitExporter.TryExportMaterial(m, textureExporter, out dst)) return dst;
            if (UrpUniUnlitExporter.TryExportMaterial(m, textureExporter, out dst)) return dst;

            UniGLTFLogger.Log($"Material `{m.name}` fallbacks.");
            return FallbackExporter.ExportMaterial(m, textureExporter);
        }
    }
}