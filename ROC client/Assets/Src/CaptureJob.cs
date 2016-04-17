using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Src
{
    class CaptureJob
    {
        // Frame in which is stocked the camera video
        // Do not access until isDone is set to true
        private Mat frame = new Mat();
        public Mat getFrame()
        {
            return frame;
        }


        // Lock allowing to check is the thread is finished
        public bool isDone
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_IsDone;
                }
                return tmp;
            }
            set
            {
                lock (m_Handle)
                {
                    m_IsDone = value;
                }
            }
        }

        private Capture captureVideo = null;
        private System.Threading.Thread m_Thread = null;
        private bool m_IsDone = false;
        private object m_Handle = new object();
       
        public CaptureJob(string rtspAddr)
        {
            captureVideo = new Capture(rtspAddr);
        }

        public CaptureJob(int rtspAddr)
        {
            captureVideo = new Capture(rtspAddr);
        }

        public void Start()
        {
            m_Thread = new System.Threading.Thread(GetFrame);
            m_Thread.Start();
        }

        public void Abort()
        {
            m_Thread.Abort();
            if (captureVideo != null)
            {
                captureVideo.Dispose();
                captureVideo = null;
            }
        }

        public bool Update()
        {
            if (isDone)
                return true;
            else
                return false;
        }

        private void GetFrame()
        {
            while (captureVideo != null)
            {
                if (isDone == false && (frame = captureVideo.QueryFrame()) == null)
                {
                    UnityEngine.Debug.Log("ERROR : QueryFrame failed.");
                }
                else if (frame != null)
                {
                    isDone = true;
                }
            }
        }
    }
}
