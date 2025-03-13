using UnityEngine;

namespace CodeBase.Tools.AudioPool.Scripts.AudioSystem
{
    public class SoundBuilder
    {
        private readonly SoundHub _soundHub;
        private Vector3 _position = Vector3.zero;
        private bool _randomPitch;

        public SoundBuilder(SoundHub soundHub) => _soundHub = soundHub;

        public SoundBuilder WithPosition(Vector3 position)
        {
            _position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            _randomPitch = true;
            return this;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null)
            {
                Debug.LogError("SoundData is null");
                return;
            }

            if (!_soundHub.CanPlaySound(soundData)) return;

            SoundEmitter soundEmitter = _soundHub.Get();
            soundEmitter.Initialize(soundData, _soundHub);
            soundEmitter.transform.position = _position;
            soundEmitter.transform.parent = _soundHub.transform;

            if (_randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            if (soundData.frequentSound)
            {
                soundEmitter.Node = _soundHub.FrequentSoundEmitters.AddLast(soundEmitter);
            }

            soundEmitter.Play();
        }
    }
}