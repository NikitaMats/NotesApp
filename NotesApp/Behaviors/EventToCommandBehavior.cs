using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesApp.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        private Delegate _eventHandler;
        private EventInfo _eventInfo;

        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty InputConverterProperty =
            BindableProperty.Create(nameof(Converter), typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(object),
                typeof(EventToCommandBehavior),
                null);

        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent();
        }

        protected override void OnDetachingFrom(View bindable)
        {
            UnregisterEvent();
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent()
        {
            if (string.IsNullOrWhiteSpace(EventName))
                return;

            _eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);

            if (_eventInfo == null)
                throw new ArgumentException($"EventToCommandBehavior: Can't register the '{EventName}' event.");

            var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod(nameof(OnEvent));
            _eventHandler = methodInfo.CreateDelegate(_eventInfo.EventHandlerType, this);
            _eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
        }

        private void UnregisterEvent()
        {
            if (_eventInfo != null && _eventHandler != null)
                _eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);

            _eventInfo = null;
            _eventHandler = null;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            if (Command == null)
                return;

            var resolvedParameter = CommandParameter ?? eventArgs;

            if (Converter != null)
                resolvedParameter = Converter.Convert(resolvedParameter, typeof(object), null, null);

            if (Command.CanExecute(resolvedParameter))
                Command.Execute(resolvedParameter);
        }
    }
}