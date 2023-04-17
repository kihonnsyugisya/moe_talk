using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataModel : MonoBehaviour
{

    private int todayDate = 0;
    private int lastDate;
    [HideInInspector] public LOGIN_TYPE judge_type;

    void Awake()
    {
        DateTime now = DateTime.Now;//端末の現在時刻の取得        
        todayDate = now.Year * 10000 + now.Month * 100 + now.Day;//日付を数値化　2020年9月1日だと20200901になる

        //前回ログイン時の日付データをロード データがない場合はFIRST_USER_LOGINで0
        lastDate = PlayerPrefs.GetInt(PREFES_KEY.LAST_GET_DATA.ToString(), (int)LOGIN_TYPE.FIRST_USER_LOGIN);


        //前回と今回の日付データ比較

        if (lastDate < todayDate)//日付が進んでいる場合
        {
            judge_type = LOGIN_TYPE.TODAY_LOGIN;
        }
        else if (lastDate == todayDate)//日付が進んでいない場合
        {
            judge_type = LOGIN_TYPE.ALREADY_LOGIN;
        }
        else if (lastDate > todayDate)//日付が逆転している場合
        {
            judge_type = LOGIN_TYPE.ERROR_LOGIN;
        }
        Debug.Log(judge_type.ToString());
        //今回取得した日付をセーブ
        PlayerPrefs.SetInt(PREFES_KEY.LAST_GET_DATA.ToString(), todayDate);
        PlayerPrefs.Save();
    }

    public int LoadLife(PREFES_KEY key)
    {
        return PlayerPrefs.GetInt(key.ToString());
    }

    public void SaveLife(PREFES_KEY key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
        PlayerPrefs.Save();
    }

    public void LoadLog()
    {

    }

    public void SaveLog()
    {

    }

    public void ResetPrefes()
    {
        Debug.Log("delete all");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }



}

public enum LOGIN_TYPE
{
    FIRST_USER_LOGIN, //初回ログイン
    TODAY_LOGIN,      //ログイン
    ALREADY_LOGIN,    //ログイン済
    ERROR_LOGIN       //不正ログイン
}

public enum PREFES_KEY
{
    LAST_GET_DATA,
    PAID_LIFE,
    FREE_LIFE,
}