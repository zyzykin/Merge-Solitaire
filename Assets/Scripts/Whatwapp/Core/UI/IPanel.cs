using System;

namespace Whatwapp.Core.UI
{
    public interface IPanel
    {
        public bool IsVisible { get; }

        public void Show(Action onShowComplete = null);
        public void Hide(Action onHideComplete = null);
    }
    
    public interface IPanel<T> : IPanel
    {
        public void SetData(T context);
    }
}