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
        private String _rtspAddr = null;
        private int _cameraDevice = -1;
        private Thread m_Thread = null;
        private bool m_IsDone = false;
        private object m_Handle = new object();
        public EventWaitHandle _newItemEvent = null;
        public ManualResetEvent _exitThreadEvent = null;

        public EventWaitHandle ExitThreadEvent
        {
            get { return _exitThreadEvent; }
        }
        public EventWaitHandle NewItemEvent
        {
            get { return _newItemEvent; }
        }

        public void initEvent()
        {
            _newItemEvent = new AutoResetEvent(false);
            _exitThreadEvent = new ManualResetEvent(false);
        }

        public CaptureJob(string rtspAddr)
        {
            this.initEvent();

            this._rtspAddr = rtspAddr;
            captureVideo = new Capture(this._rtspAddr);
            frame = new Mat();
            Debug.Log(rtspAddr);
            Debug.Log("new allocate");
        }

        public CaptureJob(int camera)
        {
            this.initEvent();
            this._cameraDevice = camera;
            captureVideo = new Capture(this._cameraDevice);
            frame = new Mat();
            Debug.Log(camera);
            Debug.Log("new allocate");
        }

        public void Start()
        {
            m_Thread = new Thread(retrieveVideoFrame);
            m_Thread.Start();
            Debug.Log("new allocate start");
        }

        public void ReleaseCamera()
        {
            _exitThreadEvent.Set();
            if (captureVideo != null)
            {
                captureVideo.Dispose();
                captureVideo = null;
            }
            _exitThreadEvent.Close();
            _newItemEvent.Close();
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
            while (_exitThreadEvent.WaitOne(0, false) || captureVideo != null)
            {
                try
                {
                    if (isDone == false)
                    {
                        frame = null;
                        // Debug.Log("retrieve frame");
                        frame = captureVideo.QueryFrame();
                        isDone = true;
                    }
                    else
                        _newItemEvent.WaitOne();
                }
                catch (Exception ex)
                {
                    Debug.Log("CAMERA EXCEPTION");
                }
            };
        }
    }
}
