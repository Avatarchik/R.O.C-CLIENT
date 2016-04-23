using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading;


namespace Assets.Src
{
    class CaptureJob
    {
        // Frame in which is stocked the camera video
        // Do not access until isDone is set to true
        private Mat frame = null;
        public Mat getFrame()
        {
            return frame;
        }

        public void releaseFrame()
        {
            frame.Dispose();
            frame = null;
            frame = new Mat();
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
        private Thread m_Thread = null;
        private bool m_IsDone = false;
        private object m_Handle = new object();
       
        public CaptureJob(string rtspAddr)
        {
            captureVideo = new Capture(rtspAddr);
            frame = new Mat();
            Debug.Log("new allocate");
        }

        public CaptureJob(int rtspAddr)
        {
            captureVideo = new Capture(rtspAddr);
            frame = new Mat();

            Debug.Log(rtspAddr);
            Debug.Log("new allocate");
        }

        public void Start()
        {
            m_Thread = new Thread(retrieveVideoFrame);
            m_Thread.Start();
            Debug.Log("new allocate start");
        }

        public void Abort()
        {
            m_Thread.Abort();
            if (captureVideo != null)
            {
                captureVideo.Dispose();
                captureVideo = null;
            }
            if (this.frame != null)
            {
                this.frame.Dispose();
                this.frame = null;
            }
        }

        public bool Update()
        {
            if (isDone)
                return true;
            else
                return false;
        }

        private void retrieveVideoFrame()
        {
            while (true)
            {
                if (isDone == false)
                {
                   // Debug.Log("retrieve frame");
                    captureVideo.Retrieve(frame, 0);
                    if (frame != null)
                    {
                        isDone = true;
                    }
                }
            };
        }
    }
}
