namespace Web.Logic.Services
{
    using Common.Models.Enums;
    using System;

    public interface IUnityAppManager
    {
        void UpdateState(UnityAppState state);
        TimeSpan GetTimeSpan();
    }
}
