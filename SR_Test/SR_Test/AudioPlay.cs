using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SR_Test
{
    class AudioPlay
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        //audio file play
        public void playMP3File(string audioFilename)
        {
            mciSendString("open \"" + audioFilename + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);

        }

        public void playSimpleSound(string audioFilename)
        {
            SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\User\Desktop\AdminLF\C#\음성인식\SR_Test\SR_Test\" + audioFilename);
            simpleSound.Play();
        }
         
    }
}
