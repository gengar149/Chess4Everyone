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
//using OpenAI_Api;
//using System.Diagnostics.Eventing.Reader;

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

        //OpenAI_API.OpenAIAPI openai;
        string prompt = "Look at the string and fix it to have the correct format so the move can be completed for a chess game." +
            "The correct format is:" +
            "move {location} to {location}" +
            "example: move a2 to a3." +
            "There might be errors in the text, for example it might say 'move a 2 too a 4', you should correct it to say 'move a2 to a4'." +
            "The string might only say 'a2 a4', you should correct this to 'move a2 to a4' so the move is correctly completed." +
            "If you are unsure of what the intended move is supposed to be, re state their move and say you dont undersatand.";




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



        void Start()
        {
            //var aopenAI = new OpenAIApi("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");
        }

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

                if (SceneManager.GetActiveScene().name == "MenuScene")
                    Command(response);
                else
                    MovePiece(response);



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

        void MovePiece(string move)
        {
            text.text = move;
            //string newMove = FixMove(move);
            if (restartWords.Contains(move))
                gameManager.Reload();
            else if (TTsWords.Contains(move))
                toggle.isOn = !toggle.isOn;
            else if (exitWords.Contains(move))
                gameManager.BackMenu();

            

            else if (move.StartsWith("move"))
            {
                string[] parts = move.Split(' ');


                if (parts[2] == "to")
                {

                    int p1 = ParseMove(parts[1][0]);
                    int p2 = Convert.ToInt32(parts[1][1].ToString());

                    int p3 = ParseMove(parts[3][0]);
                    int p4 = Convert.ToInt32(parts[3][1].ToString());



                    board.allCells[p1][p2-1].currentPiece.GetComponent<BasePiece>().TTsSelect();
                    
                    Cell targetCell = board.allCells[p3][p4-1];
                    board.allCells[p1][p2-1].currentPiece.GetComponent<BasePiece>().TTsDrop(targetCell);
                }

                else
                    UnityEngine.Debug.Log("Not valid move");
            }
        }

        int ParseMove(char letter)
        {
            int num = 0;
            if (letter == 'a')
                num = 0;
            if (letter == 'b')
                num = 1;
            if (letter == 'c')
                num = 2;
            if (letter == 'd')
                num = 3;
            if (letter == 'e')
                num = 4;
            if (letter == 'f')
                num = 5;
            if (letter == 'g')
                num = 6;
            if (letter == 'h')
                num = 7;
            UnityEngine.Debug.Log(num);
            return num;
        }
        /*
        string FixMove(string move)
        {

            var response = await openai.CreateChatCompletion(new CreateChatCompletionRequest
            {
                Model = "gpt-4o-mini",
                Messages = new List<ChatMessage>()
                {
                    new ChatMessage
                    {
                        Role = "user",
                        ContentDisposition = prompt
                    }
                }
            });

            if (response.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                return message.Content.Trim();
                
                message.Content = message.Content.Trim();
            }


        }*/




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