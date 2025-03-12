using UnityEngine;
using UnityEngine.UIElements;

namespace DebugUI
{
    public abstract class DebugUIBuilderBase : MonoBehaviour
    {
        [SerializeField] protected UIDocument uiDocument;
        protected DebugUIBuilder _builder;

        protected abstract void Configure(IDebugUIBuilder builder);

        protected virtual void Awake()
        {
            _builder = new DebugUIBuilder();
            _builder.ConfigureWindowOptions(options =>
            {
                options.Title = GetType().Name;
            });

            Configure(_builder);
            _builder.BuildWith(uiDocument);
        }
    }
}