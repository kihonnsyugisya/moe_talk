using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Scsho : MonoBehaviour
{

    [MenuItem("Tools/Screenshot %F3", false)]
    public static void CaptureScreenshot()
    {
        //プロジェクト名を取得する
        string productName = Application.productName;

        //デスクトップにプロジェクト名のフォルダを作るようにする。
        string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), productName);

        //ファイル名を指定する。
        DateTime now = DateTime.Now;
        string fileName = string.Format("{0}_{1}x{2}_{3}{4:D2}{5:D2}{6:D2}{7:D2}{8:D2}.png", productName, Screen.width, Screen.height, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        string path = Path.Combine(directory, fileName);

        //ディレクトリが存在していなかったらディレクトリを作成する。
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        //スクショを保存する
        ScreenCapture.CaptureScreenshot(path);
        Debug.LogFormat("Screenshot Saved : {0}", path);
    }
}