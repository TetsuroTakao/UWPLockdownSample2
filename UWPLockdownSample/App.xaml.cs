﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPLockdownSample.Feature;
using UWPLockdownSample.Model;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWPLockdownSample
{
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために起動されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動の要求とプロセスの詳細を表示します。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
            // ウィンドウがアクティブであることだけを確認してください
            if (rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 以前中断したアプリケーションから状態を読み込みます
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    App_Startup();
                    // ナビゲーション スタックが復元されない場合は、最初のページに移動します。
                    // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                    //構成します
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 現在のウィンドウがアクティブであることを確認します
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。
        /// アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
        /// かかわらず、アプリケーションの状態が保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
            deferral.Complete();
        }

        void App_Startup()
        {
            //[Logging file save to Microsoft Azure] explains how to send log file of next step to Azure Blob Storage.
            List<LogModel> logs = new List<LogModel>();
            LogModel message = new LogModel() { Message = "check app files ...", OccurredTime = DateTime.Now, OperatorName = typeof(App).Name, LogType = LogType.Information };
            logs.Add(message);
            //Check existing of log file.
            //if (!logFile.Exists) current execution is first time execution, so create four accounts.
            Logging logging = new Logging();
            if (logging.ReadLogs().Count == 0)
            {

                message = new LogModel() { Message = "Current run is first time , so creating four accounts.", OccurredTime = DateTime.Now, OperatorName = typeof(App).Name, LogType = LogType.Information };
                logs.Add(message);
                logging.AppendWrite(logs);
                //[Local account creation] explains how to create these four accounts.
                //CoreApplication.Exit();
            }
            else //if (logFile.Exists) check current user
            {
                message = new LogModel() { Message = "check signin account and navigating for account", OccurredTime = DateTime.Now, OperatorName = typeof(App).Name, LogType = LogType.Information };
                logs.Add(message);
                logging.AppendWrite(logs);
                //[Desktop UI control] explains how to check current user
                //if (UserName == "maintenanceOperator") navigate to MaintenanceWindow
                //[Launch app by correct account] explains how to navigate use logon script
                //if (UserName == "appOperator") quiet this app, and set logonscript to run end user application as maintenance mode. and run end user application as maintenance mode at this time, it run automatically from next sign in.
                //if (UserName == "appUser") quiet this app, and set logonscript to run end user application as normal mode. and run end user application as normal mode at this time, it run automatically from next sign in.
            }
        }
    }
}
