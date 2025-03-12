using System.Collections.Generic;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

namespace _Tools
{
    public class HapticVariablesManager : MonoBehaviour
    {
        [SerializeField] private List<HapticValueChangeUi> _valuesChangeUis;
        [SerializeField] private float _timeStep;
        [SerializeField] private float _powerStep;
        
        private Dictionary<HapticValueChangeUi, ChangeableHaptic> _hapticTypesMap =
            new Dictionary<HapticValueChangeUi, ChangeableHaptic>();

        private static List<ChangeableHaptic> _haptics = new List<ChangeableHaptic>();
        private static ChangeableHaptic _selection;
        private static ChangeableHaptic _light;
        private static ChangeableHaptic _medium;
        private static ChangeableHaptic _heavy;
        private static ChangeableHaptic _rigid;
        private static ChangeableHaptic _soft;
        private static HapticPatterns.Preset _success;
        private static HapticPatterns.Preset _failure;
        private static HapticPatterns.Preset _warning;
        private static bool _isInited;

        public static HapticPatterns.Preset Selection => _selection.HapticPreset;
        public static HapticPatterns.Preset Light => _light.HapticPreset;
        public static HapticPatterns.Preset Medium => _medium.HapticPreset;
        public static HapticPatterns.Preset Heavy => _heavy.HapticPreset;
        public static HapticPatterns.Preset Rigid => _rigid.HapticPreset;
        public static HapticPatterns.Preset Soft => _soft.HapticPreset;
        public static HapticPatterns.Preset Success => _success;
        public static HapticPatterns.Preset Failure => _failure;
        public static HapticPatterns.Preset Warning => _warning;

        public static void Init()
        {
            if (_isInited)
            {
                return;
            }

            _isInited = true;
            _light = new ChangeableHaptic(HapticPatterns.PresetType.LightImpact, 0.025f, 0.049f);
            _medium = new ChangeableHaptic(HapticPatterns.PresetType.MediumImpact, 0.055f, 0.050f);
            _heavy = new ChangeableHaptic(HapticPatterns.PresetType.HeavyImpact, 0.190f, 0.405f);
            _rigid = new ChangeableHaptic(HapticPatterns.PresetType.RigidImpact, 0.025f, 0.49f);
            _soft = new ChangeableHaptic(HapticPatterns.PresetType.SoftImpact, 0.2f, 0.16f);
            _selection = new ChangeableHaptic(HapticPatterns.PresetType.Selection, 0.025f, 0.21f);

            _haptics.Add(_light);
            _haptics.Add(_medium);
            _haptics.Add(_heavy);
            _haptics.Add(_rigid);
            _haptics.Add(_soft);
            _haptics.Add(_selection);

            /* _light = new HapticPatterns.Preset(HapticPatterns.PresetType.LightImpact, new float[] { 0.000f, 0.085f },
                new float[] { 0.0f, 0.1f  });

            _medium = new HapticPatterns.Preset(HapticPatterns.PresetType.MediumImpact, new float[] { 0.000f, 0.15f },
                new float[] { 0.0f, 0.2f });

            _heavy = new HapticPatterns.Preset(HapticPatterns.PresetType.HeavyImpact, new float[] { 0.0f, 0.3f },
                new float[] { 0.0f, 0.4f });

            _rigid = new HapticPatterns.Preset(HapticPatterns.PresetType.RigidImpact, new float[] { 0.0f, 0.05f},
                new float[] { 0.0f, 0.45f});

            _soft = new HapticPatterns.Preset(HapticPatterns.PresetType.SoftImpact, new float[] { 0.000f, 0.2f },
                new float[] { 0.0f, 0.15f });

            _selection = new HapticPatterns.Preset(HapticPatterns.PresetType.Selection, new float[] { 0.0f, 0.04f },
                new float[] { 0.0f, 0.271f });*/

            _failure = new HapticPatterns.Preset(HapticPatterns.PresetType.Failure,
                new float[] { 0.0f, 0.080f, 0.120f, 0.200f, 0.240f, 0.400f, 0.440f, 0.480f },
                new float[] { 0.0f, 0.470f, 0.000f, 0.470f, 0.000f, 1.000f, 0.000f, 0.157f });

            _success = new HapticPatterns.Preset(HapticPatterns.PresetType.Success,
                new float[] { 0.0f, 0.040f, 0.080f, 0.240f },
                new float[] { 0.0f, 0.157f, 0.000f, 1.000f });

            _warning = new HapticPatterns.Preset(HapticPatterns.PresetType.Warning,
                new float[] { 0.0f, 0.120f, 0.240f, 0.280f },
                new float[] { 0.0f, 1.000f, 0.000f, 0.470f });
        }

        private void Start()
        {
            Init();
            _hapticTypesMap.Clear();
            for (int i = 0; i < _haptics.Count; i++)
            {
                _hapticTypesMap.Add(_valuesChangeUis[i], _haptics[i]);
                _valuesChangeUis[i].OnClick += ReactOnButtonClick;
                _valuesChangeUis[i].SetView(_haptics[i].View);
            }
        }

        private void ReactOnButtonClick(HapticValueChangeUi ui, HapticCheatActionType cheatActionType)
        {
            var haptic = _hapticTypesMap[ui];
            
            switch (cheatActionType)
            {
                case HapticCheatActionType.AddTime:
                    haptic.AddTime(_timeStep);
                    break;
                case HapticCheatActionType.RemoveTime:
                    haptic.RemoveTime(_timeStep);
                    break;
                case HapticCheatActionType.AddPower:
                    haptic.AddPower(_timeStep);
                    break;
                case HapticCheatActionType.RemovePower:
                    haptic.RemovePower(_timeStep);
                    break;
                case HapticCheatActionType.Test:
                    HapticPatterns.PlayPreset(haptic.Type);
                    break;
            }
            
            ui.SetView(haptic.View);
        }
    }
}