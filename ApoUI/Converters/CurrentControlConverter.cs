//using ApoUI.Views;
//using LiveCharts;
//using LiveCharts.Wpf;
//using System;
//using System.Collections.Generic;
//using System.Globalization;

//namespace ApoUI
//{
//    /// <summary>
//    /// Converts (sorted) dictionary to LiveChart's series collection
//    /// </summary>
//    public class CurrentControlConverter : BaseConverter<CurrentControlConverter>
//    {
//        protected ThresholdingControl _ThresholdingControl = new ThresholdingControl();
//        protected BorderTypeControl _BorderControl = new BorderTypeControl();

//        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var controlType = (CurrentControlEnum)value;
//            switch (controlType)
//            {
//                case CurrentControlEnum.Thresholding:
//                    return _ThresholdingControl;
//                case CurrentControlEnum.Border:
//                    return _BorderControl;
//                default:
//                    throw new NotImplementedException();
//            }
//            //switch (sideMenuType)
//            //{
//            //    case SideMenuModel.Histogram:
//            //        return _HistogramView;
//            //    case SideMenuModel.Info:
//            //        return _HistogramView;
//            //    default:
//            //        return "Not implemented yet";
//            //}
//        }

//        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
