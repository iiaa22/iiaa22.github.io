using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户数据管理器
/// </summary>
public class UserDataManager
{
    /// <summary>
    /// 用户数据
    /// </summary>
    private static List<BaseUserData> userDataList = new();
    /// <summary>
    /// 系统设置
    /// </summary>
    public static SystemSettings systemSettings;

    /// <summary>
    /// 存档信息
    /// </summary>
    public struct UserDataInfo
    {
        /// <summary>
        /// 存档位
        /// </summary>
        public int slot;
        /// <summary>
        /// 存档名
        /// </summary>
        public string name;
        /// <summary>
        /// 是否为自动存档
        /// </summary>
        public bool isAutoSave;
    }

    /// <summary>
    /// 加载系统设置。
    /// </summary>
    public static void LoadSystemSettings()
    {
        systemSettings = JsonUtility.FromJson<SystemSettings>(PlayerPrefs.GetString("SystemSettings"));
        if (systemSettings == null)
        {
            systemSettings = new();
            PlatformCommonConfigItem platformCommonConfigItem = ConfigManager.GetConfigDataByName<PlatformCommonConfigItem>(PlatformManager.Current.ToString());
            systemSettings.Language = platformCommonConfigItem == null ? LanguageType.SChinese : platformCommonConfigItem.defaultLanguage;
        }
        else
        {
            systemSettings.AfterLoad();
        }
        systemSettings.OnDataChanged += data => SaveSystemData();
    }

    /// <summary>
    /// 获得用户数据。
    /// </summary>
    /// <typeparam name="T">用户数据类型</typeparam>
    /// <returns>用户数据</returns>
    public static T GetUserData<T>() where T : BaseUserData, new()
    {
        foreach (BaseUserData userData in userDataList)
        {
            if (userData is T)
            {
                return userData as T;
            }
        }
        T data = new();
        userDataList.Add(data);
        return data;
    }

    /// <summary>
    /// 保存系统数据。
    /// </summary>
    public static void SaveSystemData()
    {
        systemSettings.BeforeSave();
        PlayerPrefs.SetString("SystemSettings", JsonUtility.ToJson(systemSettings));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存存档。
    /// </summary>
    /// <param name="slot">存档位</param>
    /// <param name="dataName">存档名</param>
    public static void SaveUserData(int slot, string dataName)
    {
        userDataList.ForEach(userData => userData.BeforeSave());
        PlayerPrefs.SetString($"UserData{slot}", JsonUtility.ToJson(userDataList));
        PlayerPrefs.SetString($"UserDataInfo{slot}", JsonUtility.ToJson(new UserDataInfo()
        {
            slot = slot,
            name = dataName,
            isAutoSave = false
        }));
        PlayerPrefs.Save();
        if (systemSettings.SaveSlotList.IndexOf(slot) < 0) systemSettings.SaveSlotList.Add(slot);
    }

    /// <summary>
    /// 保存自动存档。
    /// </summary>
    public static void AutoSaveUserData()
    {
        userDataList.ForEach(userData => userData.BeforeSave());
        int slot = (systemSettings.LastAutoSaveSlot + 1) % systemSettings.MaxAutoSaveCount;
        PlayerPrefs.SetString($"AutoSaveUserData{slot}", JsonUtility.ToJson(userDataList));
        PlayerPrefs.SetString($"AutoSaveUserDataInfo{slot}", JsonUtility.ToJson(new UserDataInfo()
        {
            slot = slot,
            name = $"自动存档{slot}",
            isAutoSave = true
        }));
        PlayerPrefs.Save();
        if (systemSettings.AutoSaveSlotList.IndexOf(slot) < 0) systemSettings.AutoSaveSlotList.Add(slot);
        systemSettings.LastAutoSaveSlot = slot;
        Debug.Log($"游戏已自动存档，档位：{slot}");
    }

    /// <summary>
    /// 读取存档信息列表。
    /// </summary>
    /// <returns>存档信息列表</returns>
    public static List<UserDataInfo> LoadUserDataInfoList(bool isAutoSave)
    {
        List<UserDataInfo> list = new();
        if (isAutoSave)
        {
            foreach (int slot in systemSettings.AutoSaveSlotList)
            {
                list.Add(JsonUtility.FromJson<UserDataInfo>(PlayerPrefs.GetString($"AutoSaveUserDataInfo{slot}")));
            }
        }
        else
        {
            foreach (int slot in systemSettings.SaveSlotList)
            {
                list.Add(JsonUtility.FromJson<UserDataInfo>(PlayerPrefs.GetString($"UserDataInfo{slot}")));
            }
        }
        return list;
    }

    /// <summary>
    /// 读取存档。
    /// </summary>
    /// <param name="userDataInfo">存档信息</param>
    public static void LoadUserData(UserDataInfo userDataInfo)
    {
        string key = userDataInfo.isAutoSave ? $"AutoSaveUserData{userDataInfo.slot}" : $"UserData{userDataInfo.slot}";
        userDataList = JsonUtility.FromJson<List<BaseUserData>>(PlayerPrefs.GetString(key));
        userDataList.ForEach(userData => userData.AfterLoad());
    }

    /// <summary>
    /// 删除存档。
    /// </summary>
    /// <param name="userDataInfo">存档信息</param>
    public static void DeleteUserData(UserDataInfo userDataInfo)
    {
        if (userDataInfo.isAutoSave)
        {
            systemSettings.AutoSaveSlotList.Remove(userDataInfo.slot);
            PlayerPrefs.DeleteKey($"AutoSaveUserDataInfo{userDataInfo.slot}");
            PlayerPrefs.DeleteKey($"AutoSaveUserData{userDataInfo.slot}");
        }
        else
        {
            systemSettings.SaveSlotList.Remove(userDataInfo.slot);
            PlayerPrefs.DeleteKey($"UserDataInfo{userDataInfo.slot}");
            PlayerPrefs.DeleteKey($"UserData{userDataInfo.slot}");
        }
        PlayerPrefs.Save();
    }
}
