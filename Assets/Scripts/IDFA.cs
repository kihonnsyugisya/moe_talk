#if UNITY_IOS

using System.Collections;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.iOS;

public sealed class IDFA : MonoBehaviour
{
    private void Awake()
    {
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
    }
}

#endif