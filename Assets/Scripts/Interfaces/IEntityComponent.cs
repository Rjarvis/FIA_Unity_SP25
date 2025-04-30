using System;

namespace Interfaces
{
    public interface IEntityComponent
    {
        void SetContext(Context context);
        Context GetContext();
        bool TryGetComponent<T>(out T component) where T : class;
    }
}