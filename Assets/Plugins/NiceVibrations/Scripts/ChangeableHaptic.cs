using System.Globalization;
using Lofelt.NiceVibrations;

namespace _Tools
{
    public class ChangeableHaptic
    {
        public HapticPatterns.Preset HapticPreset;
        private float _time;
        private float _power;
        public HapticPatterns.PresetType Type { get; private set; }
        public string View
        {
            get
            {
                string time = _time.ToString(CultureInfo.InvariantCulture);
                if (time.Length > 4)
                    time = time.Substring(0, 5);
                
                string power = _power.ToString(CultureInfo.InvariantCulture);
                if (power.Length > 4)
                    power = power.Substring(0, 5);
                
                return Type + ";  time = " + time + ";  power = " + power;
            }
        }

        public ChangeableHaptic(HapticPatterns.PresetType type, float time, float power)
        {
            _power = power;
            _time = time;
            Type = type;
            SetPreset(_time, _power, Type);
        }

        private void SetPreset(float time, float power, HapticPatterns.PresetType presetType)
        {
            HapticPreset = new HapticPatterns.Preset(presetType, new float[] { 0.000f, time, time * 1.5f },
                new float[] { 0.0f, power, 0.0f });
        }

        public void AddTime(float addTime)
        {
            _time += addTime;
            SetPreset(_time, _power, Type);
        }

        public void RemoveTime(float removeTime)
        {
            _time -= removeTime;
            SetPreset(_time, _power, Type);
        }

        public void AddPower(float addPow)
        {
            _power += addPow;
            SetPreset(_time, _power, Type);
        }

        public void RemovePower(float removePow)
        {
            _power -= removePow;
            SetPreset(_time, _power, Type);
        }
    }
}