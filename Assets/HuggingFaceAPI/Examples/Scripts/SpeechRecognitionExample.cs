using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System;
using System.Security.Principal;
using System.Net.Mime;


namespace HuggingFace.API.Examples {
    public class SpeechRecognitionExample : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI text;

        private AudioClip clip;
        private byte[] bytes;
        private bool recording;

        [SerializeField] GameObject canvas;
        [SerializeField] GameManager gameManager;
        [SerializeField] Toggle toggle;

        [SerializeField] Board board;


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

        List<string> TTsWords = new List<string>
        {
            "tts",
            "text to speech",
            "text to switch",
            "texas beach",
            "text the speech"

        };

        List<string> restartWords = new List<string>
        {
            "reset",
            "restart",
            "we start"
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

            HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
                text.color = Color.white;

                response = response.Replace(",", "").Replace(".", "").Replace("?", "").Replace("!", "").Replace("'", "").Replace("-", " ").ToLower().Trim();
                text.text = response;

                if (SceneManager.GetActiveScene().name == "MenuScene")
                    Command(response);
                else
                    MovePiece(response);

            }, error => {
                text.color = Color.red;
                text.text = error;

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

        void MovePiece(string move)
        {
            text.text = move;
            if (restartWords.Contains(move))
                gameManager.Reload();
            else if (TTsWords.Contains(move))
                toggle.isOn = !toggle.isOn;
            else if (exitWords.Contains(move))
                gameManager.BackMenu();

            else if (move.StartsWith("move"))
            {
                string[] parts = move.Split(' ');
                if (parts[2] == "to" || parts[1] == "2")
                {
                    int p1 = ParseMove(parts[1][0]);
                    int p2 = Convert.ToInt32(parts[1][1].ToString());

                    int p3 = ParseMove(parts[3][0]);
                    int p4 = Convert.ToInt32(parts[3][1].ToString());

                    board.allCells[p1][p2-1].currentPiece.GetComponent<BasePiece>().TTsSelect();
                    
                    Cell targetCell = board.allCells[p3][p4-1];
                    board.allCells[p1][p2-1].currentPiece.GetComponent<BasePiece>().TTsDrop(targetCell);
                }
            }


            else
            {
                string[] parts = move.Split(' ');
                if (parts[1] == "to" || parts[1] == "2")
                {
                    int p1 = ParseMove(parts[0][0]);
                    int p2 = Convert.ToInt32(parts[0][1].ToString());

                    int p3 = ParseMove(parts[2][0]);
                    int p4 = Convert.ToInt32(parts[2][1].ToString());

                    board.allCells[p1][p2 - 1].currentPiece.GetComponent<BasePiece>().TTsSelect();

                    Cell targetCell = board.allCells[p3][p4 - 1];
                    board.allCells[p1][p2 - 1].currentPiece.GetComponent<BasePiece>().TTsDrop(targetCell);
                }
            }




        }

        int ParseMove(char letter)
        {
            int num = 0;
            if (letter == 'a')
                num = 0;
            else if (letter == 'b')
                num = 1;
            else if (letter == 'c')
                num = 2;
            else if (letter == 'd')
                num = 3;
            else if (letter == 'e')
                num = 4;
            else if (letter == 'f')
                num = 5;
            else if (letter == 'g')
                num = 6;
            else if (letter == 'h')
                num = 7;
            else
                return 8;
            return num;
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