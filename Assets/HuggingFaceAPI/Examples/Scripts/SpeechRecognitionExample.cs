using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HuggingFace.API.Examples {
    public class SpeechRecognitionExample : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI text;

        private AudioClip clip;
        private byte[] bytes;
        private bool recording;

        [SerializeField] GameObject canvas;

        List<string> sandboxWords = new List<string>
        {
            "one",
            "sandbox",
            "sand box"
        };

        List<string> versusWords = new List<string>
        {
            "two",
            "versus ai",
            "verse ai",
            "versus",
            "ai",
            "verse"

        };

        List<string> exitWords = new List<string>
        {
            "three",
            "exit",
            "quit",
            "leave",
            "escape",
            "quit game",
            "leave game",
            "exit game",
            "get me out of here"
        };

        List<string> wordListWords = new List<string>
        {
            "word list",
            "wordless",
            "were listening"
        };


        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (recording)
                    StopRecording();
                else
                    StartRecording();
            }

            if (recording && Microphone.GetPosition(null) >= clip.samples) {
                StopRecording();
            }
        }

        private void StartRecording() {
            text.color = Color.white;
            text.text = "Recording...";
            //startButton.interactable = false;
            //stopButton.interactable = true;
            clip = Microphone.Start(null, false, 10, 44100);
            recording = true;
        }

        private void StopRecording() {
            var position = Microphone.GetPosition(null);
            Microphone.End(null);
            var samples = new float[position * clip.channels];
            clip.GetData(samples, 0);
            bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
            recording = false;
            SendRecording();
        }

        private void SendRecording() {
            text.color = Color.yellow;
            text.text = "Sending...";
            //stopButton.interactable = false;
            HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
                text.color = Color.white;
                //text.text = response;


                response = response.Replace(",", "").Replace(".", "").Replace("?", "").Replace("!", "").Replace("'", "").ToLower().Trim();
                text.text = response;
                Command(response);

                //startButton.interactable = true;
            }, error => {
                text.color = Color.red;
                text.text = error;
                //startButton.interactable = true;
            });
        }

        void Command(string command)
        {

            if (sandboxWords.Contains(command))
            {
                Sandbox();
            }
            else if (versusWords.Contains(command))
            {
                VsAI();
            }
            else if (exitWords.Contains(command))
            {
                ExitGame();
            }
            else if (wordListWords.Contains(command))
            {
                WordList();
            }
            else
            {
                Close();
            }

        }


        void Sandbox()
        {
            text.text = "Going to Sandbox";
            canvas.GetComponent<MainMenu>().PlayGame();
        }

        void VsAI()
        {
            text.text = "Going to Versus AI";
            canvas.GetComponent<MainMenu>().PlayIA();
        }

        void ExitGame()
        {
            text.text = "Quitting";
            Application.Quit();
        }

        void WordList()
        {
            canvas.GetComponent<MainMenu>().OpenSettings();
        }

        void Close()
        {
            canvas.GetComponent<MainMenu>().CloseSettings();
        }


        private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
            using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
                using (var writer = new BinaryWriter(memoryStream)) {
                    writer.Write("RIFF".ToCharArray());
                    writer.Write(36 + samples.Length * 2);
                    writer.Write("WAVE".ToCharArray());
                    writer.Write("fmt ".ToCharArray());
                    writer.Write(16);
                    writer.Write((ushort)1);
                    writer.Write((ushort)channels);
                    writer.Write(frequency);
                    writer.Write(frequency * channels * 2);
                    writer.Write((ushort)(channels * 2));
                    writer.Write((ushort)16);
                    writer.Write("data".ToCharArray());
                    writer.Write(samples.Length * 2);

                    foreach (var sample in samples) {
                        writer.Write((short)(sample * short.MaxValue));
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}