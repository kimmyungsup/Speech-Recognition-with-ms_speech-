using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;


namespace SR_Test
{
    public partial class Form1 : Form
    {

        Bitmap ani_normal = new Bitmap("giphy4.gif");
        Bitmap ani_greet = new Bitmap("giphy6.gif");
        bool currentlyAnimating = false;

        AudioPlay audioPlay = new AudioPlay();
        SqlConnect sql = new SqlConnect();

        public Form1()
        {
            InitializeComponent();

            initRS();
            initTTS();
        }


        // gif animation thread function
        void animateRun()
        {
            if(!currentlyAnimating)
            {
                Thread.Sleep(2000);
            }
            PictureBox1.Image = ani_greet;
            Thread.Sleep(2000);
            PictureBox1.Image = ani_normal;
            currentlyAnimating = false;

            return;
        }

     
        // initialize Speech Recognization 
        public void initRS()
        {
            try
            {
                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new CultureInfo("ko-KR"));

                Grammar g = new Grammar("input.xml");
                sre.LoadGrammar(g);
                
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception e)
            {
                label1.Text = "init RS Error : " + e.ToString();
            }
        }

        //SpeechSynthesizer tts;
        // do not use MS text to speech
        // initTTS is for Audio play Test
        public void initTTS()
        {
            try 
            {
                audioPlay.playSimpleSound("ba_1.wav");
                PictureBox1.Image = ani_normal;
               
            }
            catch(Exception e)
            {
                label1.Text = "Audio initializing error : " + e.ToString();
            }
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            label1.Text = e.Result.Text;

            string sqldata = sql.ReturnVoiceData(e.Result.Text);
            if (sqldata != "error")
            {
                
                audioPlay.playSimpleSound(sqldata + ".wav");
                currentlyAnimating = true;
                Thread t1 = new Thread(new ThreadStart(animateRun));
                t1.Start();

                // way of using thread with parameter
                // Thread t1 = new Thread(new ParameterizedThreadStart(animateRun));
                // or Thread t1 = new Thread(() => animateRun(10, 20, 30));  : with more parameter
                // t1.Start(parameter)
               
            }
            
        }

        // 프로세스 실행
        private static void doProgram(string filename, string arg)
        {
            ProcessStartInfo psi;
            if(arg.Length != 0)
                psi = new ProcessStartInfo(filename, arg);
            else
                psi = new ProcessStartInfo(filename);
            Process.Start(psi);
        }

        // 프로세스 종료
        private static void closeProcess(string filename)
        {
            Process[] myProcesses;
            // Returns array containing all instances of Notepad.
            myProcesses = Process.GetProcessesByName(filename);
            foreach (Process myProcess in myProcesses)
            {
                myProcess.CloseMainWindow();
            }
        }

    }
}
