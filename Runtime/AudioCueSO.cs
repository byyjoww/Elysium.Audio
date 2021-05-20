using System;
using UnityEngine;

namespace Elysium.Audio
{
	/// <summary>
	/// A collection of audio clips that are played in parallel, and support randomisation.
	/// </summary>
	[CreateAssetMenu(fileName = "newAudioCue", menuName = "Scriptable Objects/Audio/Audio Cue")]
	public class AudioCueSO : ScriptableObject
	{
		public bool looping = false;
		[SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

		public bool IsEmpty()
		{
			return _audioClipGroups == null || _audioClipGroups.Length < 1;
		}

		public AudioClip[] GetClips()
		{
			AudioClip[] resultingClips = new AudioClip[_audioClipGroups.Length];

			for (int i = 0; i < _audioClipGroups.Length; i++)
			{
				resultingClips[i] = _audioClipGroups[i].GetNextClip();
			}

			return resultingClips;
		}
	}

	/// <summary>
	/// Represents a group of AudioClips that can be treated as one, and provides automatic randomisation or sequencing based on the <c>SequenceMode</c> value.
	/// </summary>
	[Serializable]
	public class AudioClipsGroup
	{
		public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
		public AudioClip[] audioClips;

		private int _nextClipToPlay = -1;
		private int _lastClipPlayed = -1;

		/// <summary>
		/// Chooses the next clip in the sequence, either following the order or randomly.
		/// </summary>
		/// <returns>A reference to an AudioClip</returns>
		public AudioClip GetNextClip()
		{
			// Fast out if there is only one clip to play
			if (audioClips.Length == 1) { return audioClips[0]; }				

			if (_nextClipToPlay == -1)
			{
				// Index needs to be initialised: 0 if Sequential, random if otherwise
				_nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
			}
			else
			{
				if (sequenceMode == SequenceMode.Random)
                {
					_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
				}
				else if (sequenceMode == SequenceMode.RandomNoImmediateRepeat)
                {
					do
					{
						_nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
					}
					while (_nextClipToPlay == _lastClipPlayed);
				}
				else if (sequenceMode == SequenceMode.Sequential)
				{
					_nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
				}
			}

			_lastClipPlayed = _nextClipToPlay;

			return audioClips[_nextClipToPlay];
		}

		public enum SequenceMode
		{
			Random,
			RandomNoImmediateRepeat,
			Sequential,
		}
	}
}