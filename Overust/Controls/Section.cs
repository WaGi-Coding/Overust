using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Overust.Controls
{
    public class Section: ContentControl
    {
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(Section), new PropertyMetadata(null));



        //public object HeaderContent
        //{
        //    get { return (object)GetValue(HeaderContentProperty); }
        //    set { SetValue(HeaderContentProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty HeaderContentProperty =
        //    DependencyProperty.Register("HeaderContent", typeof(object), typeof(Section), new PropertyMetadata(null));
    }
}
