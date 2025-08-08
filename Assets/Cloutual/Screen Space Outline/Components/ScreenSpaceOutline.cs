using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;
namespace Cloutual.Screen_Space_Outline {
	public class ScreenSpaceOutline : ScriptableRendererFeature {

		private readonly RenderPassEvent _renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
		// public Material _outlineMaterial;
		// private CustomRenderPass _outlinePass;

		public Material _normalMaterial;
		public Material _depthMaterial;
		private DrawTextureBufferPass _drawTextureBufferPass;

		public Material _outlineBlitMaterial;
		private OutlineBlitPass _outlineBlitPass;

		public override void Create() {
			// _outlinePass = new CustomRenderPass();
			// _outlinePass.renderPassEvent = _renderPassEvent;

			_drawTextureBufferPass = new DrawTextureBufferPass();
			_drawTextureBufferPass.renderPassEvent = _renderPassEvent;

			_outlineBlitPass = new OutlineBlitPass();
		}

		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
			if (renderingData.cameraData.cameraType != CameraType.Game
				|| _normalMaterial == null || _depthMaterial == null
				|| _outlineBlitMaterial == null
				// || _outlineMaterial == null
			) return;
			var outlineVolume = VolumeManager.instance.stack.GetComponent<OutlineVolume>();
			if (outlineVolume == null || !outlineVolume.IsActive()) return;

			// _outlinePass.Setup(_outlineMaterial, outlineVolume);
			// renderer.EnqueuePass(_outlinePass);

			_drawTextureBufferPass.Setup(_normalMaterial, _depthMaterial, outlineVolume._LayerMask);
			renderer.EnqueuePass(_drawTextureBufferPass);

			_outlineBlitPass.Setup(_outlineBlitMaterial, outlineVolume);
			_outlineBlitPass.renderPassEvent = outlineVolume._PassQueue switch {
				OutlineVolume.PassQueue.BeforePostProcessing => RenderPassEvent.BeforeRenderingPostProcessing,
				_ => RenderPassEvent.AfterRenderingPostProcessing,
			};
			renderer.EnqueuePass(_outlineBlitPass);
		}
	}

	public class DrawTextureBufferPass : ScriptableRenderPass {
		private const string passName = "Pass_Draw normal and depth texture";
		private const string NORMAL_TEXTURE_NAME = "_OutlineNormalTexture";
		private const string DEPTH_TEXTURE_NAME = "_OutlineDepthTexture";
		private static readonly int _normalTextureId = Shader.PropertyToID(NORMAL_TEXTURE_NAME);
		private static readonly int _depthTextureId = Shader.PropertyToID(DEPTH_TEXTURE_NAME);
		private static readonly ShaderTagId[] _shaderTagIds = {
			new ShaderTagId("UniversalForwardOnly"),
			new ShaderTagId("UniversalForward"),
			new ShaderTagId("SRPDefaultUnlit"),
			new ShaderTagId("LightweightForward"),
			new ShaderTagId("DepthNormals")
		};
		private static List<ShaderTagId> _shaderTagIdList = new List<ShaderTagId>();

		private Material _normalMaterial;
		private Material _depthMaterial;
		private LayerMask _layerMask;

		public DrawTextureBufferPass() {
			_shaderTagIdList = new List<ShaderTagId>(_shaderTagIds);
		}
		private class PassData {
			public RendererListHandle _rendererList;
		}
		public void Setup(Material normalMaterial, Material depthMaterial, LayerMask layerMask) {
			_normalMaterial = normalMaterial;
			_depthMaterial = depthMaterial;
			_layerMask = layerMask;
		}
		private void Execute(PassData data, RasterGraphContext context) {
			context.cmd.ClearRenderTarget(RTClearFlags.Color, Color.black, 1, 0);
			context.cmd.DrawRendererList(data._rendererList);
		}
		private RendererListHandle GenerateRendererList(RenderGraph renderGraph, ContextContainer frameData, Material overrideMaterial) {

			UniversalRenderingData universalRenderingData = frameData.Get<UniversalRenderingData>();
			UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
			UniversalLightData lightData = frameData.Get<UniversalLightData>();

			var sortFlags = cameraData.defaultOpaqueSortFlags;
			RenderQueueRange renderQueueRange = RenderQueueRange.opaque;
			FilteringSettings filterSettings = new FilteringSettings(renderQueueRange, _layerMask);

			DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(_shaderTagIdList, universalRenderingData, cameraData, lightData, sortFlags);
			drawSettings.overrideMaterial = overrideMaterial;

			var param = new RendererListParams(universalRenderingData.cullResults, drawSettings, filterSettings);
			return renderGraph.CreateRendererList(param);
		}
		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
			ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Normal);
			var universalResourceData = frameData.Get<UniversalResourceData>();
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName, out var passData)) {
				TextureDesc desc = renderGraph.GetTextureDesc(universalResourceData.activeColorTexture);
				desc.name = NORMAL_TEXTURE_NAME;
				TextureHandle normalTexture = renderGraph.CreateTexture(desc);
				passData._rendererList = GenerateRendererList(renderGraph, frameData, _normalMaterial);
				builder.UseRendererList(passData._rendererList);
				builder.SetRenderAttachment(normalTexture, 0);
				builder.SetGlobalTextureAfterPass(normalTexture, _normalTextureId);
				builder.SetRenderFunc<PassData>(Execute);
			}
			using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName, out var passData)) {
				TextureDesc desc = renderGraph.GetTextureDesc(universalResourceData.activeColorTexture);
				desc.name = DEPTH_TEXTURE_NAME;
				TextureHandle depthTexture = renderGraph.CreateTexture(desc);
				passData._rendererList = GenerateRendererList(renderGraph, frameData, _depthMaterial);
				builder.UseRendererList(passData._rendererList);
				builder.SetRenderAttachment(depthTexture, 0);
				builder.SetGlobalTextureAfterPass(depthTexture, _depthTextureId);
				builder.SetRenderFunc<PassData>(Execute);
			}
		}
	}
	public class OutlineBlitPass : ScriptableRenderPass {
		private const string passName = "Pass_Outline Blit";
		private const string _DiagonalSampleName = "_DIAGONAL_SAMPLE";
		private const string _BidirectionalSampleName = "_BIDIRECTIONALSAMPLE";
		private static readonly int _XOffset = Shader.PropertyToID("_XOffset");
		private static readonly int _YOffset = Shader.PropertyToID("_YOffset");
		private static readonly int _DepthSensitivity = Shader.PropertyToID("_DepthSensitivity");
		private static readonly int _NormalSensitivity = Shader.PropertyToID("_NormalSensitivity");
		private static readonly int _OutlineColor = Shader.PropertyToID("_OutlineColor");
		private Material _outlineMaterial;
		public void Setup(Material outlineMaterial, OutlineVolume outlineVolume) {
			_outlineMaterial = outlineMaterial;
			requiresIntermediateTexture = true;
			LocalKeyword diagonalSample = new LocalKeyword(_outlineMaterial.shader, _DiagonalSampleName);
			LocalKeyword bidirectionalSample = new LocalKeyword(_outlineMaterial.shader, _BidirectionalSampleName);
			switch (outlineVolume._SampleType) {
				case OutlineVolume.SampleType.Diagonal:
					_outlineMaterial.SetKeyword(diagonalSample, true);
					_outlineMaterial.SetKeyword(bidirectionalSample, false);
					break;
				case OutlineVolume.SampleType.Bidirectional:
					_outlineMaterial.SetKeyword(diagonalSample, false);
					_outlineMaterial.SetKeyword(bidirectionalSample, true);
					break;
				default:
					_outlineMaterial.SetKeyword(diagonalSample, false);
					_outlineMaterial.SetKeyword(bidirectionalSample, false);
					break;
			}
			_outlineMaterial.SetInt(_XOffset, outlineVolume._XOffset);
			_outlineMaterial.SetInt(_YOffset, outlineVolume._YOffset);
			_outlineMaterial.SetFloat(_DepthSensitivity, outlineVolume._DepthSensitivity);
			_outlineMaterial.SetFloat(_NormalSensitivity, outlineVolume._NormalSensitivity);
			_outlineMaterial.SetColor(_OutlineColor, outlineVolume._OutlineColor);
		}
		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
			var universalResourceData = frameData.Get<UniversalResourceData>();
			var source = universalResourceData.activeColorTexture;
			var destinationDesc = renderGraph.GetTextureDesc(source);
			destinationDesc.name = $"DestinationDesc-{passName}";
			destinationDesc.clearBuffer = false;
			TextureHandle destination = renderGraph.CreateTexture(destinationDesc);
			RenderGraphUtils.BlitMaterialParameters para = new RenderGraphUtils.BlitMaterialParameters(source, destination, _outlineMaterial, 0);
			renderGraph.AddBlitPass(para, passName: passName);
			renderGraph.AddCopyPass(destination, source, passName: passName);
		}
	}
}