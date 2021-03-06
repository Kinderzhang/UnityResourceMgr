﻿using UnityEngine;
using System.Collections;
using AutoUpdate;

public class TestDownload : MonoBehaviour {

    public UIButton m_BtnDownload = null;
    public UISlider m_Progress = null;
	public UILabel m_LbDown = null;

	private double m_LastM = 0;
	private double m_LastTotalM = 0;

    void InitUI()
    {
        if (m_BtnDownload != null)
        {
            EventDelegate.Add(m_BtnDownload.onClick, OnBtnDownClick);
        }

        AutoUpdateMgr.Instance.OnStateChanged = StateChanged;
        AutoUpdateMgr.Instance.OnError = OnAutoUpdateError;
    }

    void OnAutoUpdateError(AutoUpdateErrorType errType, int code)
    {
        Debug.LogFormat("OnUpdateError: errType {0:D} code {0:D}", (int)errType, code);
    }

    void OnBtnDownClick()
    {
		AutoUpdateMgr.Instance.StartAutoUpdate("http://192.168.1.102:1983/outPath");
    }

    void StateChanged(AutoUpdateState state)
    {
        Debug.LogFormat("AutoUpdate ChangeState: {0:D}", (int)state);

        if (state == AutoUpdateState.auEnd)
        {
            // 進入遊戲
            Debug.Log("Enter Game!!!");
        } else if (state == AutoUpdateState.auFinished)
        {
            // 下載完成
            Debug.Log("Res Update Finished!!!");
            ResourceMgr.Instance.AutoUpdateClear();
            ResourceMgr.Instance.LoadConfigs(OnResLoad);
        }
    }

    void OnResLoad(bool isFinished)
    {
        if (isFinished)
        {
            AssetLoader loader = ResourceMgr.Instance.AssetLoader as AssetLoader;
            if (loader != null)
            {
                
            }
        }
    }

	// Use this for initialization
	void Start () {
        InitUI();
        ResourceMgr.Instance.LoadConfigs(OnResLoad);
    }
	
	// Update is called once per frame
	void Update () {
        TimerMgr.Instance.ScaleTick(Time.deltaTime);
        TimerMgr.Instance.UnScaleTick(Time.unscaledDeltaTime);

        AutoUpdateMgr.Instance.Update();
        float value = AutoUpdateMgr.Instance.DownProcess;
        if (m_Progress != null)
            m_Progress.value = value;

		if (m_LbDown != null)
		{
			if (m_LastM != AutoUpdateMgr.Instance.CurDownM || m_LastTotalM != AutoUpdateMgr.Instance.TotalDownM)
			{
				m_LastM = AutoUpdateMgr.Instance.CurDownM;
				m_LastTotalM = AutoUpdateMgr.Instance.TotalDownM;
				string s = string.Format("{0}/{1} M", m_LastM.ToString(), m_LastTotalM.ToString());
				m_LbDown.text = s;
			}
		}
    }
}
