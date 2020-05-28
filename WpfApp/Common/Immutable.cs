using System;

namespace WpfApp.Common
{
    public abstract class Immutable<T>
        where T : Immutable<T>
    {
        protected T Update(Action<T> updateAction)
        {
            var clone = this.Clone();
            updateAction(clone);
            return clone;
        }

        protected T Update(Action<T> updateAction, Func<bool> updateCheck)
        {
            if (!updateCheck())
            {
                return (T)this;
            }

            var clone = this.Clone();
            updateAction(clone);
            return clone;
        }

        protected T Clone()
        {
            return (T)this.MemberwiseClone();
        }
    }
}
