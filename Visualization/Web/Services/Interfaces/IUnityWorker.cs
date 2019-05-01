using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services
{
    public interface IUnityWorker
    {
        UnityState State { get; set; }

        void LogState();
    }

    public enum UnityState
    {
        ACTIVE,
        NOT_ESTABLISHED
    }
}
