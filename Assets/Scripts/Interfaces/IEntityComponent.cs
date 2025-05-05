using System;
using UnityEngine;

namespace Interfaces
{
    public interface IEntityComponent
    {
        GameObject GetGameObject();
        Transform GetTransform();
        void SetContext(Context context);
        Context GetContext();
        bool TryGetComponent<T>(out T component) where T : class;
    }
}