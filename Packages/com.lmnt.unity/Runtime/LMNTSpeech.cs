using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LMNT
{
    public class LMNTSpeech : MonoBehaviour
    {
        private AudioSource _audioSource;
        private string _apiKey;
        private List<Voice> _voiceList;
        private DownloadHandlerAudioClip _handler;

        public string voice;
        public string dialogue;

        void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            _apiKey = LMNTLoader.LoadApiKey();
            _voiceList = LMNTLoader.LoadVoices();

            foreach (Voice voice in _voiceList) {
                print(voice.name);
            }
        }

        public IEnumerator Prefetch()
        {
            // Dispose of the previous handler to clear old data
            if (_handler != null)
            {
                _handler.Dispose();
                _handler = null;
            }

            // Validate and prepare the data
            string voiceId = LookupByName(voice);
            if (string.IsNullOrEmpty(voiceId))
            {
                Debug.LogError("Voice is null or not found in the voice list.");
                yield break;
            }

            if (string.IsNullOrEmpty(dialogue))
            {
                Debug.LogError("Dialogue is null or empty.");
                yield break;
            }

            // Prepare a new request with the updated dialogue text
            WWWForm form = new WWWForm();
            form.AddField("voice", voiceId);
            form.AddField("text", dialogue);

            using (UnityWebRequest request = UnityWebRequest.Post(Constants.LMNT_SYNTHESIZE_URL, form))
            {
                _handler = new DownloadHandlerAudioClip(Constants.LMNT_SYNTHESIZE_URL, AudioType.WAV);
                request.SetRequestHeader("X-API-Key", _apiKey);
                request.SetRequestHeader("X-Client", "unity/0.1.0");
                request.downloadHandler = _handler;

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        _audioSource.clip = _handler.audioClip;
                    }
                    catch
                    {
                        Debug.LogError("Error processing audio clip in LMNTSpeech Prefetch function.");
                    }
                }
                else
                {
                    Debug.LogError("Audio fetch failed: " + request.error);
                }
            }
        }


        public IEnumerator Talk()
        {
            // Fetch the latest audio based on updated dialogue
            yield return StartCoroutine(Prefetch());

            // Wait until the new audio clip is loaded before playing
            if (_audioSource.clip != null)
            {
                _audioSource.Play();
            }
        }

        // Method to set new dialogue text
        public void SetDialogue(string newDialogue)
        {
            dialogue = newDialogue;
            //StartCoroutine(Talk());
        }

        private string LookupByName(string name)
        {
            return _voiceList.Find(v => v.name == name)?.id;
        }
    }
}