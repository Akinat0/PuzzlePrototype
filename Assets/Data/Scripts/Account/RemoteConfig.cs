using System;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class RemoteConfig
{
    struct userAttributes { }
    struct appAttributes { }
    
    public RemoteConfig()
    {
        ConfigManager.FetchCompleted += ApplyRemoteSettings;
        ConfigManager.FetchConfigs(new userAttributes(), new appAttributes());
    }

    public event Action<RemoteConfigInfo> OnDefaultValuesLoaded;
    public event Action<RemoteConfigInfo> OnCachedValuesLoaded;
    public event Action<RemoteConfigInfo> OnRemoteValuesLoaded;

    void ApplyRemoteSettings(ConfigResponse response)
    {
        RemoteConfigInfo configInfo = new RemoteConfigInfo();
        
        configInfo.version = ConfigManager.appConfig.GetString("version");

        ParseTiers(ref configInfo);
        
        Debug.Log($"<color=yellow>Config was loaded. RemoteConfigOrigin is {response.requestOrigin}, config version: {configInfo.version}</color>");
        
        switch (response.requestOrigin)
        {
            case ConfigOrigin.Default:
                OnDefaultValuesLoaded?.Invoke(configInfo);
                return;
            case ConfigOrigin.Cached:
                OnCachedValuesLoaded?.Invoke(configInfo);
                return;
            case ConfigOrigin.Remote:
                OnRemoteValuesLoaded?.Invoke(configInfo);
                return;
        }
        
    }

    void ParseTiers(ref RemoteConfigInfo configInfo)
    {
        Tier[] tiers = Account.Tiers;
        
        List<TierInfo> tierInfos = new List<TierInfo>(tiers.Length);
            
        for (int i = 0; i < tiers.Length; i++)
        {
            Tier tier = tiers[i];
            
            string tierJson = ConfigManager.appConfig.GetString($"Tier{tier.ID}", string.Empty);
            
            if (string.IsNullOrEmpty(tierJson))
            {
                Debug.LogWarning($"Can't load tier {tier.ID}");
                continue;
            }

            TierInfo tierInfo = JsonUtility.FromJson<TierInfo>(tierJson);
            
            if(tierInfo == null){
                
                Debug.LogWarning($"Can't parse tier {tier.ID}, json : {tierJson}");
                continue;
            }

            tierInfos.Add(tierInfo);
            
            tier.Parse(tierInfo);
        }

        configInfo.tierInfos = tierInfos.ToArray();
    }
}

public struct RemoteConfigInfo
{
    public string version;

    public TierInfo[] tierInfos;
}