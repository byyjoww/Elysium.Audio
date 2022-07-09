#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Elysium.Audio
{
    [CustomEditor(typeof(AudioChannelListener))]
    public class AudioChannelListenerEditor : Editor
    {
        private const string greenIndicator = "d_winbtn_mac_max";
        private const string yellowIndicator = "d_winbtn_mac_min";
        private const string redIndicator = "d_winbtn_mac_close";

        private AudioChannelListener listener = default;

        protected virtual GUIStyle WindowStyle
        {
            get
            {
                GUIStyle style = GUI.skin.window;
                style.padding.top = 10;
                style.padding.bottom = 10;
                style.padding.left = 10;
                style.padding.right = 10;
                return style;
            }
        }

        private void OnEnable()
        {
            listener = (AudioChannelListener)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Channels", EditorStyles.label);
            EditorGUILayout.BeginVertical(WindowStyle);
            var channels = listener.openAudioChannels;
            int numOfChannels = channels != null ? channels.Count() : 0;
            for (int i = 0; i < numOfChannels; i++)
            {
                IAudioChannelInternal channel = channels.ElementAt(i).Key;
                DrawChannel(channel, i);
            }

            if (numOfChannels == 0) { GUILayout.Label("Empty"); }
            EditorGUILayout.EndVertical();
        }

        private static void DrawChannel(IAudioChannelInternal _channel, int _index)
        {
            GUIContent icon = EditorGUIUtility.IconContent(_channel.IsPlaying ? greenIndicator : redIndicator);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField(icon, GUILayout.Width(20));
            GUILayout.Space(10);

            GUILayout.Label("Channel:", GUILayout.Width(60));
            GUILayout.TextField($"{_index + 1}");
            GUILayout.Space(10);

            GUILayout.Label("Clip:", GUILayout.Width(30));
            GUILayout.TextField($"{_channel.Clip?.name}");
            GUILayout.Space(10);

            GUILayout.Label("Mixer Group:", GUILayout.Width(80));
            GUILayout.TextField($"{_channel.Config?.Group?.name}");
            GUILayout.Space(10);

            GUILayout.Label("Looping:", GUILayout.Width(55));
            GUILayout.TextField($"{_channel.IsLooping}");

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif