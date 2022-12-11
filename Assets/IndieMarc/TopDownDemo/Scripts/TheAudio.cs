using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Главный скрипт управления звуком
namespace IndieMarc.TopDown
{
    public class TheAudio : MonoBehaviour
    {
        public string[] preload_channels;

        private static TheAudio _instance;

        private Dictionary<string, AudioSource> channels = new Dictionary<string, AudioSource>();

        void Awake()
        {
            _instance = this;

            foreach (string channel in preload_channels)
            {
                CreateChannel(channel);
            }
        }

        // channel: Два звука на одном канале никогда не будут играть вместе
        // priority: если false, то не будет играть если уже воспроизводится,
        // если true заменит текущий звук на канале.
        public void PlaySound(string channel, AudioClip sound, float vol = 0.8f, bool priority = false)
        {
            if (string.IsNullOrEmpty(channel) || sound == null)
                return;

            AudioSource source = GetChannel(channel);

            // Создает канал, если тот не существует

            if (source == null)
                source = CreateChannel(channel); 
            if (source)
            {
                if (priority || !source.isPlaying)
                {
                    source.clip = sound;
                    source.volume = vol;
                    source.Play();
                }
            }
        }

        public AudioSource GetChannel(string channel)
        {
            if (channels.ContainsKey(channel))
                return channels[channel];
            return null;
        }

        public bool DoesChannelExist(string channel)
        {
            return channels.ContainsKey(channel);
        }

        public AudioSource CreateChannel(string channel, int priority = 128)
        {
            if (string.IsNullOrEmpty(channel))
                return null;

            GameObject cobj = new GameObject("AudioChannel-" + channel);
            cobj.transform.parent = transform;
            AudioSource caudio = cobj.AddComponent<AudioSource>();
            caudio.playOnAwake = false;
            caudio.loop = false;
            caudio.priority = priority;
            channels[channel] = caudio;
            return caudio;
        }

        public static TheAudio Get()
        {
            return _instance;
        }
    }

}