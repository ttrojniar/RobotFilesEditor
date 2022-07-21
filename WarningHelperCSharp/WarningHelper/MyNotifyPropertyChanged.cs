﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WarningHelper
{
    public abstract class MyNotifyPropertyChanged : INotifyPropertyChanged
    {
	    public event PropertyChangedEventHandler PropertyChanged;

	    #region " Methods "
        /// <summary>
	    /// This method is called by the Set accessor of each property. 
	    /// The CallerMemberName attribute that is applied to the optional propertyName 
	    /// parameter causes the property name of the caller to be substituted as an argument. 
        /// </summary>
	    public virtual void NotifyPropertyChanged([CallerMemberName()] string propertyName = null)
	    {
		    if (PropertyChanged != null) {
			    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		    }
	    }

        /// <summary>
        /// Helper method for changing a field. 
        /// If the previous field differs from the new value, 
        /// an event will be raised.
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="field">Reference to the field.</param>
        /// <param name="value">New value of the field.</param>
        /// <param name="propertyName">Optional name of the property. If not used: it takes the Caller Member Name.</param>
        /// <returns>True if the field has changed.</returns>
        protected bool Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
#if DEBUG
            if ((DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())))
            {
                field = value; return true;
            }
#endif
            if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

	    #endregion
    }
}
