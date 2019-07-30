using NPCGenerator.Dto;
using System.Windows;
using System.Windows.Controls;

namespace NPCGenerator.Util
{
    public class DynamicDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if ( container is FrameworkElement element && item is DataRow model )
            {
                return (DataTemplate)element.FindResource( model.DisplayValueAs + "Template" );
            }

            return null;
        }
    }
}
