using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using System.Windows.Markup;

namespace IntentConnectWeighing
{
    class TemplateHelper
    {
        public static string getTemplateXamlCode(System.Windows.Controls.Control control)
        {
            FrameworkTemplate template = control.Template;
            string xaml = "";
            if (template != null)
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = new string(' ', 4);
                settings.NewLineOnAttributes = true;
                StringBuilder strbuild = new StringBuilder();
                XmlWriter xmlwrite = XmlWriter.Create(strbuild, settings);
                try
                {
                    xaml = strbuild.ToString();
                }
                catch (Exception exc)
                {
                    xaml = exc.Message;
                }
            }
            else
            {
                xaml = "no template";
            }
            return xaml;
        }

        public static FrameworkElement getFrameworkElementFromXaml(string path)
        {
            XmlTextReader reader = new XmlTextReader(path);
            //FileStream fs;
            //fs = new FileStream(path, FileMode.Open);
            FrameworkElement element = XamlReader.Load(reader) as FrameworkElement;
            //fs.Close();          
            return element;
        }

        public static FrameworkElement getFrameworkElementFromResource(string resourseFilePath)
        {
            Uri uri = new Uri(resourseFilePath, UriKind.RelativeOrAbsolute);
            if (uri == null) return null;
            Console.Write(uri.ToString());
            Stream stream = Application.GetResourceStream(uri).Stream;
            return XamlReader.Load(stream) as FrameworkElement;
        }

        public FrameworkElement getFrameworkElementFromString(string strXaml)
        {
            StringReader strreader = new StringReader(strXaml);
            XmlTextReader xmlreader = new XmlTextReader(strreader);
            return XamlReader.Load(xmlreader) as FrameworkElement;
        }
        /// <summary>
        /// 获取控件模板
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns></returns>
        public static ControlTemplate getControlTemplate(ResourceName name) {
         return   (ControlTemplate)App.Current.Resources[name.ToString()];
        }


        /// <summary>
        /// 获得父窗体控件
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前子控件名</param>
        /// <param name="name">要查询父控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            //DependencyObject parent2 = VisualTreeHelper.GetParent(parent);


            while (parent != null)
            {
                if (parent is T && (((T)parent).Name == name || string.IsNullOrEmpty(name)))
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
        /// <summary>
        /// 获得子控件
        /// </summary>
        /// <typeparam name="T">要获得控件类名</typeparam>
        /// <param name="obj">当前控件名</param>
        /// <param name="name">要查询子控件名</param>
        /// <returns>要获得控件类名</returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;


            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);


                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
        //---

    }
}
