
namespace IntentConnectWeighing
{
    /// <summary>
    /// description of data status 数据状态说明
    /// </summary>
    class GlobalDataStatus
    {
        #region data Synchronization Status 数据同步状态
        //SynchronizationStatus
        /// <summary>
        /// 0需要上传，以本地数据为依据
        /// </summary>
        public static readonly int AsyncUp = 0;

        /// <summary>
        /// 1 本地数据和服务器数据一至
        /// </summary>
        public static readonly int Asynced = 1;
        /// <summary>
        /// 2.需要更新数据，以服务器数据为依据
        /// </summary>
        public static readonly int AsuncDown = 2;
        #endregion

        #region Bill UP Status 过磅单上传的状态
        /// <summary>
        ///0没有上传过
        /// </summary>
        public static readonly int NoUp = 0;

        /// <summary>
        ///1. 第一次上伟完成
        /// </summary>
        public static readonly int UpOne = 1;
        /// <summary>
        /// 2.第二次上伟完成
        /// </summary>
        public static readonly int UpTwo = 2;
        #endregion

        #region Bill Weithing Status 过磅磅单过磅的状态
        //
        /// <summary>
        ///0第一次过磅完成
        /// </summary>
        public static readonly int FinishOne = 0;

        /// <summary>
        ///1. 第二次过磅完成
        /// </summary>
        public static readonly int FinishedTwo = 1;
        #endregion
    }
}
