using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntentConnectWeighing.CameraSdk;
using MyHelper;
namespace IntentConnectWeighing
{

    public class CameraHelper
    {

        public string message = string.Empty;
        public Int32 m_lPort = -1;
        public Int32 currCameraId = -1;
        public uint iLastErr;
        public bool isInitSDK;
        public bool isLogin = false;
        public bool isPreviewSuccess = false;
        public int m_lRealHandle = -1;
        public System.Windows.Forms.PictureBox mPictureBox;
        public CHCNetSDK.REALDATACALLBACK RealData = null;
        public IntPtr m_ptrRealHandle;

        public static bool InitSDK()
        {
            return CHCNetSDK.NET_DVR_Init();
        }
        /// <summary>
        /// login Camera
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns>currcamera id </returns>
        public static int loginCamera(string ip, string port, string userName, string pwd,ref CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo)
        {          
         return   CHCNetSDK.NET_DVR_Login_V30(ip, Convert.ToInt32(port), userName, pwd, ref deviceInfo);
          
        }
        public static void loginOutCamera(int currCameraId)
        {
            try
            {
                CHCNetSDK.NET_DVR_Logout(currCameraId);
            }
            catch (Exception)
            {
            }

        }
        public static bool preview(ref System.Windows.Forms.PictureBox pb, int lChannel,int currCameraId)
        {
            try
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = pb.Handle;//预览窗口
                lpPreviewInfo.lChannel = lChannel;//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = false; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 20; //播放库播放缓冲区最大缓冲帧数

                // RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数               
                //打开预览 Start live view 
                 CHCNetSDK.NET_DVR_RealPlay_V40(currCameraId, ref lpPreviewInfo, null, new IntPtr());
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public void RealDataCallBack(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser)
        {
            //下面数据处理建议使用委托的方式
            // MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            switch (dwDataType)
            {
                case CHCNetSDK.NET_DVR_SYSHEAD:     // sys head
                    if (dwBufSize > 0)
                    {
                        if (m_lPort >= 0)
                        {
                            return; //同一路码流不需要多次调用开流接口
                        }

                        //获取播放句柄 Get the port to play
                        if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "PlayM4_GetPort failed, error code= " + iLastErr;
                            // this.mPictureBox.BeginInvoke(AlarmInfo, message);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            //this.mPictureBox.BeginInvoke(AlarmInfo, message);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "PlayM4_OpenStream failed, error code= " + iLastErr;
                            //this.mPictureBox.BeginInvoke(AlarmInfo, message);
                            break;
                        }


                        //设置显示缓冲区个数 Set the display buffer number
                        if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                            // this.mPictureBox.BeginInvoke(AlarmInfo, message);
                        }

                        //设置显示模式 Set the display mode
                        if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                            // this.mPictureBox.BeginInvoke(AlarmInfo, message);
                        }

                        //设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data                      
                        if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, new PlayCtrl.DECCBFUN(DecCallbackFUN), IntPtr.Zero, 0))
                        {
                            //  this.mPictureBox.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayCallBack fail");
                        }

                        //开始解码 Start to play                       
                        if (!PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            message = "PlayM4_Play failed, error code= " + iLastErr;
                            // this.mPictureBox.BeginInvoke(AlarmInfo, message);
                            //System.Threading.Thread.Sleep(2);
                            break;
                        }
                    }
                    break;
                case CHCNetSDK.NET_DVR_STREAMDATA:     // video stream data
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            //送入码流数据进行解码 Input the stream data to decode
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                message = "PlayM4_InputData failed, error code= " + iLastErr;
                                // System.Threading.Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        //送入其他数据 Input the other data
                        for (int i = 0; i < 999; i++)
                        {
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                message = "PlayM4_InputData failed, error code= " + iLastErr;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }
        //解码回调函数
        private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2)
        {
            // 将pBuf解码后视频输入写入文件中（解码后YUV数据量极大，尤其是高清码流，不建议在回调函数中处理）
            if (pFrameInfo.nType == 3)
            {

            }
        }
        //停止预览 Stop live view 
        public static bool stopPreview(int currCameraId)
        {
            try
            {
                return CHCNetSDK.NET_DVR_StopRealPlay(currCameraId);
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// captureJpeg
        /// </summary>
        /// <param name="filePathName">图片保存路径和文件名</param>
        /// <param name="filePathName">通道号</param>
        public static bool captureJpeg(string filePathName, int currCameraId , int lChannel)
        {
            CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA();
            lpJpegPara.wPicQuality = 0; //图像质量 Image quality
            lpJpegPara.wPicSize = 0xff; //抓图分辨率 Picture size: 2- 4CIF，0xff- Auto(使用当前码流分辨率)，抓图分辨率需要设备支持，更多取值请参考SDK文档

            //JPEG抓图 Capture a JPEG picture
            return CHCNetSDK.NET_DVR_CaptureJPEGPicture(currCameraId, lChannel, ref lpJpegPara, filePathName);

        }
        /// <summary>
        /// captureBmp
        /// </summary>
        /// <param name="filePathName">图片保存路径和文件名</param>
        /// <param name="lChannel">通道号</param>
        /// <returns></returns>
        public static bool captureBmp(string filePathName, int m_lRealHandle)
        {
            //BMP抓图 Capture a BMP picture
            return !CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, filePathName);

        }

    }
}
