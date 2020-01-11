using UnityEngine.Events;

namespace PenguinCasual.Scripts.Utilities
{
    public interface ISystem
    {
        void Initialize();
        UnityAction OnInitializeDone { get; set; }
    }
}