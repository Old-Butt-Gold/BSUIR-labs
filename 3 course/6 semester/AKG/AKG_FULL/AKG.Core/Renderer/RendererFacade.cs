using System.Windows.Media;
using System.Windows.Media.Imaging;
using AKG.Core.Enum;
using AKG.Core.Objects;

namespace AKG.Core.Renderer;

public static class RendererFacade
{
    public static void Render(Scene scene, WriteableBitmap? wb, Color backgroundColor, Color foregroundColor,
        Color highlightColor, RenderMode mode)
    {
        if (wb == null) return;

        WireframeRenderer.ClearBitmap(wb, backgroundColor);

        BackgroundRenderer.RenderBackground(scene, wb);

        scene.Camera.ChangeEye();

        WireframeRenderer.RenderSelectionOutlineDoublePass(scene, wb, highlightColor);

        scene.UpdateAllModels();
        
        Rasterizer.ClearZBuffer(scene.CanvasWidth, scene.CanvasHeight, scene.Camera);

        switch (mode)
        {
            case RenderMode.Wireframe:
                foreach (var model in scene.Models)
                    WireframeRenderer.DrawWireframe(model, wb, foregroundColor, scene.Camera, 1);
                break;
            case RenderMode.FilledTrianglesLambert:
                foreach (var model in scene.Models)
                    Rasterizer.DrawFilledTriangleLambert(model, wb, foregroundColor, scene.Camera, scene.Lights);
                break;
            case RenderMode.FilledTrianglesPhong:
                // Используем готовые Normals из файлов
                foreach (var model in scene.Models)
                    Rasterizer.DrawFilledTrianglePhong(model, wb, scene.Camera, scene.Lights, true);
                break;
            case RenderMode.FilledTrianglesAverageFaceNormalPhong:
                // Используем усредненные нормали поверхности всех полигонов
                foreach (var model in scene.Models)
                    Rasterizer.DrawFilledTrianglePhong(model, wb, scene.Camera, scene.Lights, false);
                break;
            case RenderMode.Texture:
            {
                foreach (var model in scene.Models)
                    Rasterizer.DrawTexturedTriangles(model, wb, scene, false);
                break;
            }
            case RenderMode.TextureRayTracing:
            {
                foreach (var model in scene.Models)
                    Rasterizer.DrawTexturedTriangles(model, wb, scene, true);
                break;
            }
            default:
                throw new NotSupportedException("Неизвестный режим рендеринга");
        }
        
        LightRenderer.DrawLights(scene, wb);
    }
}