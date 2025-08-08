using System;
using UnityEngine;
using UnityEngine.Rendering;
namespace Cloutual.Screen_Space_Outline {
	[Serializable, VolumeComponentMenu("Custom/Outline")]
	public class OutlineVolume : VolumeComponent, IPostProcessComponent {
		[SerializeField] private BoolParameter _isActive = new BoolParameter(false);
		[SerializeField] private LayerMaskParameter _layerMask = new LayerMaskParameter(-1);
		[SerializeField] private EnumParameter<PassQueue> _passQueue = new EnumParameter<PassQueue>(PassQueue.BeforePostProcessing);
		[SerializeField] private EnumParameter<SampleType> _sampleType = new EnumParameter<SampleType>(SampleType.Simple);
		[SerializeField] private ClampedIntParameter _xOffset = new ClampedIntParameter(1, 0, 10);
		[SerializeField] private ClampedIntParameter _yOffset = new ClampedIntParameter(1, 0, 10);
		[SerializeField] private ClampedFloatParameter _depthSensitivity = new ClampedFloatParameter(0.01f, 0.005f, 0.1f);
		[SerializeField] private ClampedFloatParameter _normalSensitivity = new ClampedFloatParameter(0.5f, 0.01f, 2f);
		[SerializeField] private ColorParameter _outlineColor = new ColorParameter(Color.white, true, false, false, false);
		public LayerMask _LayerMask => _layerMask.value;
		public PassQueue _PassQueue => _passQueue.value;
		public SampleType _SampleType => _sampleType.value;
		public int _XOffset => _xOffset.value;
		public int _YOffset => _yOffset.value;
		public float _DepthSensitivity => _depthSensitivity.value;
		public float _NormalSensitivity => _normalSensitivity.value;
		public Color _OutlineColor => _outlineColor.value;

		public bool IsActive() => active &&
			_isActive.value &&
			(_xOffset.value > 0 || _yOffset.value > 0) &&
			_SampleType != SampleType.None &&
			_LayerMask.value != 0;

		public enum SampleType {
			None,
			Simple,
			Bidirectional,
			Diagonal,
		}
		public enum PassQueue {
			BeforePostProcessing,
			AfterPostProcessing,
		}
	}
}